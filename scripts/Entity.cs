using Godot;
using System;

public class Entity : Area2D
{
    [Export] public ActorShooter Actor { get; set; }
    [Export] public int Point { get; set; } = 1;
    [Export] public int Dano { get; set; } = 1;
    [Export] public int HP { get; set; } = 10;
    [Export] public float Speed { get; set; } = 250;

    [Export] float blinkInterval = 0.05f;

    protected ObjectPool objectPool;
    protected Sprite sprite;

    public void SetObjectPool()
    {
        objectPool = GetParent().GetParent().GetNode<ObjectPool>("Object_Pool");
    }

    public virtual void _On_Give_Damage(int point)
    {

    }

    public virtual void _On_Take_Damage(int dano)
    {
        HP -= dano;
        if (HP <= 0)
        {
            DeathBlink(Color.Color8(255, 255, 255, 0), 5);
        }
        else
        {
            NormalBlink(Color.Color8(255, 0, 0), 5);
        }
    }

    async void NormalBlink(Color colorToBlink, int timesToBlink = 2)
    {
        for (int i = 0; i < timesToBlink; i++)
        {
            sprite.Modulate = colorToBlink;
            await ToSignal(GetTree().CreateTimer(blinkInterval), "timeout");
            sprite.Modulate = Color.Color8(255, 255, 255);
            await ToSignal(GetTree().CreateTimer(blinkInterval), "timeout");
        }
    }

    async void DeathBlink(Color colorToBlink, int timesToBlink = 2)
    {
        for (int i = 0; i < timesToBlink; i++)
        {
            sprite.Modulate = colorToBlink;
            await ToSignal(GetTree().CreateTimer(blinkInterval), "timeout");
            sprite.Modulate = Color.Color8(255, 255, 255);
            await ToSignal(GetTree().CreateTimer(blinkInterval), "timeout");
        }
        //TODO: REMOVE THIS
        if (Actor != ActorShooter.Player)
            QueueFree();
    }
}
