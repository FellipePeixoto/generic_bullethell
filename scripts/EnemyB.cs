using Godot;
using System;

public class EnemyB : Entity
{

    [Export] float firinCooldown = 3.5f;
    [Export] float movingDuration = 1.5f;

    Timer firerCooldown;
    Timer moveTimer;
    Node2D firer;
    [Export] Vector2[] points = new Vector2[3];
    [Export] Color bulletColor;

    public override void _Ready()
    {
        SetObjectPool();
        firerCooldown = GetNode<Timer>("Timer");
        moveTimer = GetNode<Timer>("Timer2");  
        firer = GetNode<Node2D>("Firer");
        sprite = GetNode<Sprite>("Sprite");
        GlobalPosition = points[0];
        points[1].y = new Random().Next((int)points[1].y - 150, (int)points[1].y + 150);
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

        float u = 1 - (moveTimer.TimeLeft / moveTimer.WaitTime);
        if (u >= 1)
        {
            moveTimer.Start();
            return;
        }
        Vector2 p01, p12;
        p01 = (1 - u)*points[0] + u*points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        GlobalPosition = (1 - u) * p01 + u * p12;
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
