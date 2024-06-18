using Godot;
using System;

public partial class Mage : CharacterBody3D
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;
    public const float TurnSpeed = 5.0f; // Adjusted turn speed
    
   
    
        

    public bool is_walking = false;
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    { 
        AnimationPlayer playerAnimations = GetNode<AnimationPlayer>("Rig/AnimationPlayer");
        Node3D LookTarget = GetNode<Node3D>("LookTarget");
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.Y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        
        if (direction != Vector3.Zero)
        {
            is_walking = true;
            velocity.X = Mathf.MoveToward(Velocity.X, direction.X * Speed, Speed * (float)delta);
            velocity.Z = Mathf.MoveToward(Velocity.Z, direction.Z * Speed, Speed * (float)delta);
        }
        else
        {
            is_walking = false;
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed * (float)delta);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed * (float)delta);
        }

        Velocity = velocity;
        MoveAndSlide();
        RotatePlayer(delta, is_walking);
        playerAnimations.Play(velocity == Vector3.Zero ? "Idle" : "Walking_A");
    }

    private void RotatePlayer(double delta, bool is_walking)
    {
        Node3D Rig = GetNode<Node3D>("Rig");
        Node3D LookTarget = GetNode<Node3D>("LookTarget");
        LookTarget.Position = LookTarget.Position.Lerp(Velocity, (float)delta * Speed * 8);
        if (is_walking)
        {
            Rig.LookAt(LookTarget.GlobalPosition, Vector3.Up, true);
        }
    }
}
