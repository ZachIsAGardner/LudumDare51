using Godot;
using System;

public class Press : Level
{
    public override void _Process(float delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("ui_accept"))
        {
            Complete();
        }
    }
}
