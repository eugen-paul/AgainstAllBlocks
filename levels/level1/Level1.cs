using System.Collections.Generic;
using Godot;

public partial class Level1 : Node
{
    [Export]
    public PackedScene BallScene { get; set; }

    [Export]
    public int Lifes { get; set; } = 5;

    private int _ballsCount = 0;

    private Paddle _paddle;

    private readonly LinkedList<Ball> _ballsOnPaddle = new();

    public bool Pause { get; set; } = false;

    private int blockCount = 0;

    public override void _Ready()
    {
        _paddle = GetNode<Paddle>("Paddle");
        AddBall();

        var blocks = FindChildren("Block*");
        foreach (var nodeBlock in blocks)
        {
            if (nodeBlock is Block block)
            {
                block.BlockDestroyed += BoxDestroid;
                blockCount++;
            }
        }
    }

    private void BoxDestroid()
    {
        blockCount--;
    }

    private void AddBall()
    {
        var ball = BallScene.Instantiate<Ball>();
        _ballsCount++;
        ball.Position = new Vector3(_paddle.Position.X, 0.5f, _paddle.Position.Z - 1.001f);

        _ballsOnPaddle.AddLast(ball);
        AddChild(ball);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("shoot") && !Pause && _ballsOnPaddle.Count > 0)
        {
            var ball = _ballsOnPaddle.First.Value;
            _ballsOnPaddle.RemoveFirst();
            ball.Velocity = Vector3.Forward * ball.Speed;
        }
    }
}
