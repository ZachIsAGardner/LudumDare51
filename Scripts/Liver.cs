using Godot;
using System;

public class Liver : Area
{
    [Export]
    public int Hp = 1;

    [Export]
    public int Weakness = 0;

    Label3D label;
    float labelDuration = 2f;
    float labelTime = 0;

    PackedScene hitParticles = ResourceLoader.Load<PackedScene>("res://Prefabs/HitParticles.tscn");

    public override void _Ready()
    {
        base._Ready();

        Connect("body_entered", this, "OnBodyEntered");

        label = this.GetChild("Label") as Label3D;

        if (label != null) 
        {
            label.Visible = false;
        }
    }

    public void Hit()
    {
        Hp--;
        if (Hp <= 0)
        {
            GetParent().QueueFree();
        }
        else
        {
            labelTime = labelDuration;
            Particles hit = hitParticles.Instance() as Particles;
            Game.Root.AddChild(hit);
            hit.GlobalTranslation = (GetParent() as Spatial).GlobalTranslation;
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        labelTime -= delta;

        if (label != null)
        {
            label.Visible = labelTime > 0;
            label.Text = $"HP: {Hp}";
        }
    }

    public void Destroy()
    {
        GetParent().QueueFree();
    }

    void OnBodyEntered(Node other)
    {
        Bullet bullet = other as Bullet;

        if (bullet == null) return;

        if (bullet.Category != Weakness) return;

        bullet.LandedHit();

        Hit();
    }
}
