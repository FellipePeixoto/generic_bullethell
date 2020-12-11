using Godot;
using System;

public class EnemyA : Entity
{
    [Export] float weight_min = 0.01f;
    [Export] float weight_max = 0.05f;
    [Export] float firinCooldown = 3.5f;
    [Export] Color bulletColor;
    Timer firerCooldown;
    Node2D firer;    
    float weight;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetObjectPool();

        firerCooldown = GetNode<Timer>("Timer");
        firer = GetNode<Node2D>("Firer");
        sprite = GetNode<Sprite>("Sprite");
        weight = (float)new Random().NextDouble();
        weight = (weight_max - weight_min) * weight;
        weight += weight_min;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (HP <= 0)
            return;

        if (firerCooldown.IsStopped())
        {
            firerCooldown.Start(firinCooldown);
            objectPool.Shoot(this, new Vector2(0, 1), firer.GlobalPosition, bulletColor);
        }
        var p = GetParent().GetParent().GetNode("Player_Container").GetNode<Player>("Player").GlobalPosition;
        GlobalPosition = GlobalPosition.LinearInterpolate(new Vector2(p.x, GlobalPosition.y), weight);
    }


    public override void _On_Give_Damage(int point)
    {
        base._On_Give_Damage(point);
    }

    public override void _On_Take_Damage(int dano)
    {
        base._On_Take_Damage(dano);
        weight = (float)new Random().NextDouble();
        weight = (weight_max - weight_min) * weight;
        weight += weight_min;
    }
}
