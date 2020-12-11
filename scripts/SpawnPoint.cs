using Godot;
using System;
using System.Collections.Generic;

public class SpawnPoint : Node2D
{
    [Export] int maxEnemies = 6;
    [Export] float jumpLine = 64;
    [Export] float lines = 3;
    PackedScene enemyA = GD.Load("res://scenes/EnemyA.tscn") as PackedScene;
    PackedScene enemyB = GD.Load("res://scenes/EnemyB.tscn") as PackedScene;
    PackedScene enemyC = GD.Load("res://scenes/EnemyC.tscn") as PackedScene;

    public List<Entity> entities = new List<Entity>();

    Timer spawnTimer;
    float currLine = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        spawnTimer = GetNode<Timer>("Timer");
        entities.Add(GetParent().GetNode("Player_Container").GetNode<Entity>("Player"));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {   
        if (spawnTimer.IsStopped() && (entities.Count - 1) < maxEnemies)
        {
            int enemyCount = new Random().Next(3, maxEnemies);
            for(int i = 0; i <= enemyCount; i++)
            {
                int whichEnemy = new Random().Next(1, 4);
                Entity enemy;
                switch (whichEnemy)
                {
                    case 2:
                        enemy = enemyB.Instance() as Entity;
                        break;
                    case 3:
                        enemy = enemyC.Instance() as Entity;
                        break;
                    default:
                        enemy = enemyA.Instance() as Entity;
                        break;
                }
                enemy.GlobalPosition = GlobalPosition;
                GetParent().GetNode("Enemies_House").AddChild(enemy);
                entities.Add(enemy);
            }
            

            spawnTimer.Start();
            GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y - jumpLine);
            currLine++;
            if (currLine > lines)
            {
                GlobalPosition += new Vector2(GlobalPosition.x, jumpLine * lines);
                currLine = 1;
            }
        }
    }
}
