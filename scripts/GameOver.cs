using Godot;
using System;

public class GameOver : Container
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    bool loadHasCalled = false;

    public override void _Process(float delta)
    {
        if (Visible && !loadHasCalled && Input.IsActionJustPressed("ui_select"))
        {
            GetTree().ChangeScene("res://scenes/Gameplay.tscn");
            loadHasCalled = true;
        }
    }

    public void GameHasEnded()
    {
        Visible = true;
    }
}
