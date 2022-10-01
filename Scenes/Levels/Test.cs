using Godot;
using System;

public class Test : Level
{
    public override void _Ready()
    {
        base._Ready();

        Area g = this.GetChildWithName("Goal") as Area;
        g.Connect("body_shape_entered", this, "OnBodyShapeEntered");
    }

    public void OnBodyShapeEntered(RID rid, Node body, int bodyShapeIndex, int localShapeIndex)
    {
        Complete();
    }
}
