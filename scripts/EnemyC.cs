using Godot;
using System;

public class EnemyC : Entity
{
    [Export] float firinCooldown = 3.5f;
    [Export] float movingDuration = 1.5f;
    [Export] float angle = 90;
    [Export] float radius = 10;
    [Export] Color bulletColor;
    
    Timer firerCooldown;
    Node2D firer;
    bool enteringScreen = true;
    Vector2 pDesired = new Vector2();

    [Export] float medianTarget = 200;

    Vector2 target;

    public override void _Ready()
    {
        SetObjectPool();
        firerCooldown = GetNode<Timer>("Timer");
        firer = GetNode<Node2D>("Firer");
        sprite = GetNode<Sprite>("Sprite");
        //GlobalPosition = points[0];
        //points[1].y = new Random().Next((int)points[1].y - 150, (int)points[1].y + 150);
        int windowWidth = (int)ProjectSettings.GetSetting("display/window/size/width");

        //pDesired.x = (float)new Random().NextDouble() * windowWidth / 2;
        //pDesired.y = (float)new Random().NextDouble() * 100;
        target = new Vector2(medianTarget, GlobalPosition.y);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (HP <= 0)
            return;

        angle -= Speed * delta;
        Vector2 toNeed = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        Vector2 offset = toNeed * radius;
        offset.y = Mathf.Abs(offset.y);
        GlobalPosition = GlobalPosition.LinearInterpolate(target + offset, .05f);

        if (firerCooldown.IsStopped())
        {
            firerCooldown.Start(firinCooldown);
            objectPool.Shoot(this, new Vector2(0, 1), firer.GlobalPosition, bulletColor);
        }
    }

    public override void _On_Give_Damage(int point)
    {
        base._On_Give_Damage(point);
    }

    public override void _On_Take_Damage(int dano)
    {
        base._On_Take_Damage(dano);
    }
}
