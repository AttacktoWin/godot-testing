using Godot;
using System;

public class Player : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    private float maxSpeed = 600.0f;
    [Export]
    private float acceleration = 400.0f;
    [Export]
    private float deceleration = 300.0f;
    private Vector2 velocity =  Vector2.Zero;
    private Vector2 ScreenSize;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
     private void GetInput() 
     {
         var deltaX = Input.GetAxis("move_left", "move_right") * acceleration;
         if (deltaX == 0f)
         {
             deltaX = -Mathf.Clamp(deceleration * velocity.Normalized().x, -Mathf.Abs(velocity.x), Mathf.Abs(velocity.x));
         }
         velocity.x += deltaX;
         var deltaY = Input.GetAxis("move_up", "move_down") * acceleration;
         if (deltaY == 0f)
         {
             deltaY = -Mathf.Clamp(deceleration * velocity.Normalized().y, -Mathf.Abs(velocity.y), Mathf.Abs(velocity.y));
         }
         velocity.y += deltaY;

         velocity = new Vector2(
             Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed), 
             Mathf.Clamp(velocity.y, -maxSpeed, maxSpeed));
     }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        velocity = MoveAndSlide(velocity);
    }
}
