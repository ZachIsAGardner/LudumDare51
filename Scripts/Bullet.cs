using Godot;
using System;

public class Bullet : RigidBody
{
    [Export]
    public int Category = 0;

    [Export]
    float time = 3;

    public override void _Ready()
    {
        base._Ready();

        Connect("body_entered", this, "OnBodyEntered");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        time -= delta;
        this.GetChild<MeshInstance>().Scale -= (new Vector3(0.25f, 0.25f, 0.25f) * delta);
        this.GetChild<OmniLight>().OmniRange -= delta;


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
