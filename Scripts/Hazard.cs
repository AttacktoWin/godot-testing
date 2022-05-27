using Godot;
using System;

public class Hazard : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    private int damageValue = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Monitoring = true;
        Connect("body_entered", this, nameof(_on_Hazard_body_entered));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public virtual void _on_Hazard_body_entered(PhysicsBody2D body)
    {
        if (body is Player)
        {
            (body as Player).OnHit(damageValue);
        }
    }
}
