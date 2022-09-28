using Godot;
using System;

public class Player : KinematicBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector3 input = new Vector3(0, 0, 0);

        if (Input.IsActionPressed("ui_left")) input.x = -1;
        if (Input.IsActionPressed("ui_right")) input.x = 1;
        if (Input.IsActionPressed("ui_up")) input.z = -1;
        if (Input.IsActionPressed("ui_down")) input.z = 1;

        MoveAndSlide(input * 30);
    }
}
