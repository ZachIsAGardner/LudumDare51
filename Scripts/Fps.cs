using Godot;
using System;

public class Fps : KinematicBody
{
    float Hp = 6;

    Liver liver;
    Spatial apparatus;
    Spatial mesh;

    PackedScene bullet = ResourceLoader.Load<PackedScene>("res://Prefabs/Bullet.tscn");

    float speed = 12;
    float mouseSensitivity = 0.2f;
    float stickSensitivity = 3f;
    Vector3 velocity = new Vector3(0, 0, 0);
    float jumpStrength = 15f;
    float baseAcceleration = 50f;
    float baseGravity = -25f;
    Vector3 startPosition;

    float shootTime = 0;

    public override void _Ready()
    {
        base._Ready();

        Input.MouseMode = Input.MouseModeEnum.Captured;

        apparatus = this.GetChild("Apparatus") as Spatial;
        mesh = this.GetChild("Mesh") as Spatial;
        liver = this.GetChild("Liver") as Liver;
        startPosition = Translation;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        shootTime -= delta;

        Vector2 stickR = new Vector2(
            Input.GetJoyAxis(0, (int)JoystickList.Axis2),
            Input.GetJoyAxis(0, (int)JoystickList.Axis3)
        );

        if (stickR != Vector2.Zero)
        {
            CameraStickRotation();
        }

        if (Translation.y < -5)
        {
            liver.Destroy();
            Game.Load("Title");
        }

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured
                ? Input.MouseModeEnum.Visible
                : Input.MouseModeEnum.Captured;
        }

        if (Input.IsMouseButtonPressed((int)ButtonList.Left))
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        Vector3 input = new Vector3(0, 0, 0);
        if (Input.IsActionPressed("ui_up")) input.z -= 1;
        if (Input.IsActionPressed("ui_down")) input.z += 1;
        if (Input.IsActionPressed("ui_left")) input.x -= 1;
        if (Input.IsActionPressed("ui_right")) input.x += 1;
        Vector2 stick = new Vector2(
            Input.GetJoyAxis(0, (int)JoystickList.Axis0),
            Input.GetJoyAxis(0, (int)JoystickList.Axis1)
        );
        if (stick.x.Abs() <= 0.1f) stick.x = 0;
        if (stick.y.Abs() <= 0.1f) stick.y = 0;
        input.x += stick.x;
        input.z += stick.y;

        Vector3 direction = new Vector3(0, 0, 0);
        direction += apparatus.GlobalTransform.basis.z * input.z;
        direction += apparatus.GlobalTransform.basis.x * input.x;

        direction.y = 0;
        direction = direction.Normalized();

        Vector3 destinationVelocity = new Vector3(0, 0, 0);

        destinationVelocity.x = direction.x * speed;
        destinationVelocity.z = direction.z * speed;

        float acceleration = baseAcceleration;
        if (!IsOnFloor())
        {

            if (input == Vector3.Zero)
            {
                acceleration = 0;
            }
            else
            {
                acceleration = baseAcceleration / 2f;
            }
        }
        else
        {
            if (input == Vector3.Zero)
            {
                acceleration = baseAcceleration * 2;
            }
            else
            {
                acceleration = baseAcceleration;
            }
        }

        velocity.x = velocity.x.MoveTowardsWithSpeed(destinationVelocity.x, acceleration);
        velocity.z = velocity.z.MoveTowardsWithSpeed(destinationVelocity.z, acceleration);

        float gravity = baseGravity;
        if (!Input.IsActionPressed("ui_accept"))
        {
            gravity = baseGravity * 2;
        }
        velocity.y += gravity * delta;
        if (IsOnFloor())
        {
            if (Input.IsActionJustPressed("ui_accept"))
            {
                velocity.y = jumpStrength;
            }
        }

        velocity = MoveAndSlide(velocity, new Vector3(0, 1, 0));

        if (Input.IsMouseButtonPressed((int)ButtonList.Left))
        {
            if (shootTime > 0) return;
            shootTime = 0.2f;
            RigidBody instance = bullet.Instance() as RigidBody;
            Game.Root.AddChild(instance);
            Spatial spawn = (apparatus.GetChild("Spawn") as Spatial);
            instance.Translation = spawn.GlobalTranslation;
            Vector3 shootDirection = spawn.GlobalTranslation - apparatus.GlobalTranslation;
            instance.LinearVelocity = (shootDirection.Normalized() * 35f);
        }
    }

    public override void _Input(InputEvent input)
    {
        base._Input(input);
        CameraMouseRotation(input);
    }

    void CameraMouseRotation(InputEvent input)
    {
        if (Input.MouseMode != Input.MouseModeEnum.Captured) return;

        InputEventMouseMotion mouseMotion = input as InputEventMouseMotion;

        if (mouseMotion == null) return;

        Vector2 force = Vector2.Zero;
        force = mouseMotion.Relative * mouseSensitivity;
        if (force == Vector2.Zero) return;

        RotateCamera(force);
    }

    void CameraStickRotation()
    {
        Vector2 stick = new Vector2(
            Input.GetJoyAxis(0, (int)JoystickList.Axis2),
            Input.GetJoyAxis(0, (int)JoystickList.Axis3)
        );

        if (stick.x.Abs() <= 0.05f) stick.x = 0;
        if (stick.y.Abs() <= 0.05f) stick.y = 0;

        Vector2 force = Vector2.Zero;

        force = stick * stickSensitivity;

        if (force == Vector2.Zero) return;

        RotateCamera(force);
    }

    void RotateCamera(Vector2 force)
    {
        apparatus.Rotation = new Vector3(
            apparatus.Rotation.x - Mathf.Deg2Rad(force.y),
            apparatus.Rotation.y - Mathf.Deg2Rad(force.x),
            apparatus.Rotation.z
        );

        apparatus.Rotation = new Vector3(
            Mathf.Clamp(apparatus.Rotation.x, -1.5f, 1.5f),
            apparatus.Rotation.y,
            apparatus.Rotation.z
        );

        mesh.Rotation = new Vector3(
            mesh.Rotation.x,
            apparatus.Rotation.y,
            mesh.Rotation.z
        );
    }
}
