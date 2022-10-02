using Godot;
using System;
using System.Collections.Generic;

public class Liver : Area
{
    [Export]
    public int Hp = 1;

    [Export]
    public int Weakness = 0;

    Label3D label;
    float labelDuration = 2f;
    float labelTime = 0;

    MeshInstance mesh;
    Vector3 meshStartTranslation;
    float shakeDuration = 0.5f;
    float shakeTime = 0f;

    PackedScene hitParticles = ResourceLoader.Load<PackedScene>("res://Prefabs/HitParticles.tscn");

    public override void _Ready()
    {
        base._Ready();

        Connect("body_entered", this, "OnBodyEntered");

        mesh = this.GetParent().GetChild("Mesh") as MeshInstance;
        if (mesh != null)
        {
            meshStartTranslation = mesh.Translation;
        }

        label = this.GetChild("Label") as Label3D;

        if (label != null) 
        {
            label.Visible = false;
        }
    }

    public void Hit()
    {
        Hp--;

        Particles hit = hitParticles.Instance() as Particles;
        Game.Root.AddChild(hit);
        hit.GlobalTranslation = (GetParent() as Spatial).GlobalTranslation;
        
        if (Hp <= 0)
        {
            GetParent().QueueFree();
        }
        else
        {
            labelTime = labelDuration;
            shakeTime = shakeDuration;
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        labelTime -= delta;
        shakeTime -= delta;

        if (label != null)
        {
            label.Visible = labelTime > 0;
            label.Text = $"HP: {Hp}";
        }

        if (mesh != null && shakeTime > 0)
        {
            Vector3 offset = new Vector3(
                new List<int>() { -1, 0, 1 }.Random(),
                new List<int>() { -1, 0, 1 }.Random(),
                new List<int>() { -1, 0, 1 }.Random()
            );
            mesh.Translation = meshStartTranslation + (offset * (shakeTime / shakeDuration));
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
