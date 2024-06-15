using Godot;
using System;
using System.Diagnostics;

public partial class MainBackground : Node
{
    [Export]
    public PackedScene BallScene { get; set; }

    [Export]
    public int BallCount { get; set; } = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (int i = 0; i < BallCount; i++)
        {
            var ball = BallScene.Instantiate<Ball>();

            var ballSpawnLocation = GetNode<PathFollow3D>("BallSpawnPath/BallSpawnLocation");
            ballSpawnLocation.ProgressRatio = GD.Randf();

            ball.Rotation = new Vector3(0, GD.Randf() * (float)Math.PI * 2f, 0);

            ball.Position = ballSpawnLocation.Position;

            ball.Velocity = (Vector3.Forward * ball.Speed).Rotated(Vector3.Up, ball.Rotation.Y);
            // ball.Velocity = Vector3.Forward * ball.Speed;

            Debug.Print($"Rotation {ball.Rotation}, Position {ball.Position}, Velocity {ball.Velocity}");

            AddChild(ball);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
