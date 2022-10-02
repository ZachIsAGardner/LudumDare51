using Godot;
using System;

public class Bullet : RigidBody
{
    [Export]
    public int Category = 0;

    [Export]
    float time = 3;
    float duration = 3;

    MeshInstance mesh;
    Vector3 meshScale;

    OmniLight light;
    float lightRange;

    public override void _Ready()
    {
        base._Ready();

        Connect("body_entered", this, "OnBodyEntered");

        mesh = this.GetChild<MeshInstance>();
        meshScale = mesh.Scale;

        light = this.GetChild<OmniLight>();
        lightRange = light.OmniRange;

        duration = time;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        time -= delta;
        mesh.Scale = meshScale * (time / duration);
        light.OmniRange = lightRange * (time / duration);

        if (time <= 0)
        {
            QueueFree();
        }
    }

    void OnBodyEntered(Node other)
    {
        // QueueFree();
    }

    public void LandedHit()
    {
        QueueFree();
    }
}
