using Godot;
using System;


public class Player : Entity
{
    [Export] float firinCooldown;
    Timer firerCooldown;
    private Vector2 vel;
    Node2D firer;
    Label scoreLabel;
    Label lifeLabel;

    public int score;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetObjectPool();
        firerCooldown = GetNode<Timer>("Timer");
        firer = GetNode<Node2D>("Firer");
        sprite = GetNode<Sprite>("Sprite");
        scoreLabel = GetParent().GetNode<Label>("Score");
        lifeLabel = GetNode<Label>("Life");

        lifeLabel.Text = "Life: " + HP;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (HP <= 0)
            return;

        base._PhysicsProcess(delta);
        var dirVel = new Vector2(0, 0);

        if (Input.IsActionPressed("ui_select") && firerCooldown.IsStopped())
        {
            firerCooldown.Start(firinCooldown);
            objectPool.Shoot(this, new Vector2(0, -1), firer.GlobalPosition, Color.Color8(255, 255, 255));
        }
        if (Input.IsActionPressed("ui_left"))
        {
            dirVel.x = -1;
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            dirVel.x = 1;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            dirVel.y = -1;
        }
        else if (Input.IsActionPressed("ui_down"))
        {
            dirVel.y = 1;
        }
        vel = dirVel.Normalized() * Speed;
        Position += vel * delta;

        var viewRect = GetViewportRect();
        Position = new Vector2(Mathf.Clamp(Position.x, 0, viewRect.Size.x), Mathf.Clamp(Position.y, 0, viewRect.Size.y));
    }

    public override void _On_Give_Damage(int point = 1)
    {
        base._On_Give_Damage(point);
        score += point;
        scoreLabel.Text = "Score: " + score;
    }

    public override void _On_Take_Damage(int dano)
    {
        base._On_Take_Damage(dano);
        if (HP <= 0)
        {
            HP = 0;
            (GetParent().GetParent().GetNode("Container") as GameOver).GameHasEnded();
            
        }
        lifeLabel.Text = "Life: " + HP;
    }
}
