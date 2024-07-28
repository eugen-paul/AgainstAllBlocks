using Godot;
using System;

public partial class MainBackground : Node
{
    [Export]
    public PackedScene BallScene { get; set; }

    [Export]
    public int BallCount { get; set; } = 10;

    public override void _Ready()
    {
        for (int i = 0; i < BallCount; i++)
        {
            var ball = BallScene.Instantiate<Ball>();

            var ballSpawnLocation = GetNode<PathFollow3D>("BallSpawnPath/BallSpawnLocation");
            ballSpawnLocation.ProgressRatio = GD.Randf();

            ball.Rotation = new Vector3(0, GD.Randf() * (float)Math.PI * 2f, 0);

            ball.Position = new Vector3(ballSpawnLocation.Position.X, 0.5f, ballSpawnLocation.Position.Z);

            ball.Velocity = (Vector3.Forward * ball.StartSpeed).Rotated(Vector3.Up, ball.Rotation.Y);

            AddChild(ball);
        }
    }
}
