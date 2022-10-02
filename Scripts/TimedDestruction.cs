using Godot;
using System;

public class TimedDestruction : Node
{
    [Export]
    float time = 1;

    public override void _Process(float delta)
    {
        base._Process(delta);

        time -= delta;

        if (time <= 0)
        {
            GetParent().QueueFree();
        }
    }
}
