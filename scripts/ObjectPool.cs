using Godot;
using System.Linq;
using System;
using Thread = System.Threading.Thread;
using Mutex = System.Threading.Mutex;
using System.Collections.Generic;

public enum ActorShooter
{
    Player, Enemy
}

public class ObjectPool : Node2D
{
    [Export] int poolSize;
    Thread objectPooling;
    public static Mutex mutex_bullets_array = new Mutex();
    Bullet[] bullets;
    SpawnPoint spawnPointController;
    Entity player;
    PackedScene bullet = GD.Load("res://scenes/Bullet.tscn") as PackedScene;
    public override void _EnterTree()
    {
        spawnPointController = GetParent().GetNode<SpawnPoint>("SpawnPoint");
        for (int i = 0; i < poolSize; i++)
        {
            var newBullet = bullet.Instance() as Bullet;
            newBullet.Position = new Vector2(-100, -100);
            AddChild(newBullet);
        }
        
        player = GetParent().GetNode("Player_Container") as Entity;
        try
        {
            var childs = GetChildren();
            bullets = new Bullet[childs.Count];
            int j = 0;
            foreach (var x in childs)
            {
                bullets[j] = x as Bullet;
                j++;
            }
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        objectPooling = new Thread(BulletsCheck);
        objectPooling.Start();
    }

    public void Shoot(Entity shooter, Vector2 dir, Vector2 spawnPoint, Color bulletColor)
    {
        mutex_bullets_array.WaitOne();
        try
        {
            var bullet = bullets.First((x) => x.Available == true);
            bullet.Available = false;
            bullet.Shooter = shooter;
            bullet.Direction = dir;
            bullet.Position = spawnPoint;
            bullet.GetNode<Sprite>("Sprite").Modulate = bulletColor;
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
        mutex_bullets_array.ReleaseMutex();
    }

    async void BulletsCheck()
    {
        await ToSignal(GetTree(), "idle_frame");
        while (true)
        {
            foreach (Bullet b in bullets)
            {
                if (b.Available)
                    continue;

                foreach (Entity e in spawnPointController.entities)
                {
                    mutex_bullets_array.WaitOne();
                    if (b.Shooter != null && b.Shooter.Actor != e.Actor && b.OverlapsArea(e))
                    {
                        b.Shooter._On_Give_Damage(e.Point);
                        e._On_Take_Damage(b.Shooter.Dano);
                        b.Reset();
                        continue;
                    }
                    mutex_bullets_array.ReleaseMutex();
                }
                spawnPointController.entities.RemoveAll(x => x.HP <= 0);

                b.Position += b.Direction * b.Speed * GetProcessDeltaTime();

                int windowHeight = (int)ProjectSettings.GetSetting("display/window/size/height");

                if (b.GlobalPosition.y < 0 || b.GlobalPosition.y > windowHeight)
                {
                    b.Reset();
                }
            }
            await ToSignal(GetTree(), "idle_frame");
        }
    }

    public override void _ExitTree()
    {
        objectPooling.Abort();
    }
}
