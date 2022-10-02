using Godot;
using System;

public class Game : Node
{
    public static Node Root;
    public static float Delta = 1f;

    public override void _Ready()
    {
        base._Ready();

        Root = this;
        Load("Fps");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        Delta = delta;
    }

    public static void Load(string name)
    {
        Root.RemoveChildren();
        Node scene = ResourceLoader.Load<PackedScene>($"res://Scenes/{name}.tscn").Instance();
        Root.AddChild(scene);
    }
}
