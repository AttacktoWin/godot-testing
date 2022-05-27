using Godot;
using System;
using System.Collections.Generic;

public class BulletSpawner : Node2D
{
    #pragma warning disable 649
    [Export]
    private PackedScene BulletScene;
    #pragma warning restore 649
    private enum Phases
    {
        ON,
        OFF
    }
    [Export]
    private float SpawnDelay = 0.5f;
    [Export]
    private float OnTime = 2.0f;
    [Export]
    private float OffTime = 5.0f;
    [Export]
    private float TimeToLive = 10.0f;
    private List<RayCast2D[]> SpawnPositions = new List<RayCast2D[]>();
    private Timer PhaseTimer;
    private Timer SpawnTimer;
    [Export(PropertyHint.Enum, "ON,OFF")]
    private Phases CurrentPhase;
    private int currentGroupTarget = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (var node in GetChildren())
        {
            if ((node as Node).Name.Contains("SpawnPositions") && (node as Node2D).IsVisibleInTree())
            {
                RayCast2D[] positions = new RayCast2D[(node as Node).GetChildCount()];
                (node as Node).GetChildren().CopyTo(positions, 0);
                SpawnPositions.Add(positions);
            }
        }
        GD.Print(SpawnPositions.Count);

        PhaseTimer = GetNode<Timer>("PhaseTimer");
        SpawnTimer = GetNode<Timer>("SpawnTimer");

        SpawnTimer.WaitTime = SpawnDelay;

        if (CurrentPhase == Phases.ON)
        {
            PhaseTimer.WaitTime = OnTime;
            SpawnTimer.Start();
        }
        else
        {
            PhaseTimer.WaitTime = OffTime;
        }

    }

    public void _on_PhaseTimer_timeout()
    {
        PhaseTimer.Stop();
        if (CurrentPhase == Phases.ON)
        {
            SpawnTimer.Stop();
            CurrentPhase = Phases.OFF;
            PhaseTimer.WaitTime = OffTime;
        }
        else
        {
            SpawnTimer.Start();
            CurrentPhase = Phases.ON;
            PhaseTimer.WaitTime = OnTime;
        }
        PhaseTimer.Start();
    }

    public void _on_SpawnTimer_timeout()
    {
        foreach (var spawnPoint in SpawnPositions[currentGroupTarget])
        {
            var bullet = (Bullet)BulletScene.Instance();
            bullet.Position = spawnPoint.Position;
            bullet.Velocity = spawnPoint.CastTo;
            bullet.TimeToLive = TimeToLive;
            AddChild(bullet);
        }
        currentGroupTarget = (currentGroupTarget + 1) % SpawnPositions.Count;
    }
}
