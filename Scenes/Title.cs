using Godot;
using System;

public class Title : Control
{
    public override void _Process(float delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("ui_accept"))
        {
            
        }
    }
}
