using Godot;
using System;

public class Bullet : Hazard
{
    public Vector2 Velocity;
    public float TimeToLive = 10.0f;
    private Timer DespawnTimer;

    public override void _Ready()
    {
        DespawnTimer = GetNode<Timer>("DespawnTimer");
        DespawnTimer.WaitTime = TimeToLive;
        DespawnTimer.Start();
    }

    public override void _on_Hazard_body_entered(PhysicsBody2D body)
    {
        base._on_Hazard_body_entered(body);
        QueueFree();
    }

    public void _on_DespawnTimer_timeout()
    {
        QueueFree();
    }

    public override void _PhysicsProcess(float delta)
    {
        Position += Velocity * delta;
    }
}
