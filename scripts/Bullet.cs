using Godot;
using System;

public class Bullet : Area2D
{
    [Export] public float Speed { get; set; }
    public Vector2 Direction { get; set; }
    public bool Available { get; set; } = true;
    public Entity Shooter { get; set; }

    public void Reset()
    {
        Available = true;
        Shooter = null;
        Direction = Vector2.Zero;
        GlobalPosition = Vector2.One * -100;
    }
}
