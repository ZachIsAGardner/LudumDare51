using Godot;
using System;

[Tool]
public class ForceFollowParent : Camera
{
    public override void _Process(float delta)
    {
        base._Process(delta);

        Node parent = GetParent();
        Spatial spatial = parent as Spatial;
        while (spatial == null)
        {
            if (parent == null) break;
            parent = parent.GetParent();
            if (parent == null) break;
            spatial = parent as Spatial;
        }

        if (spatial == null) return;

        Translation = spatial.Translation;
        Rotation = spatial.Rotation;
    }
}
