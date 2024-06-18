using System.Collections.Generic;
using System.Diagnostics;
using Godot;

public partial class AbstractLevel : Node
{
    [Export]
    public PackedScene BallScene { get; set; }

    [Export]
    public int Lifes { get; set; } = 5;

    private int _score = 0;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            _gameHud.SetScore(Score);
        }
    }

    public int BallsCount { get; private set; } = 0;

    protected Paddle _paddle;
    protected GameHud _gameHud;

    protected readonly LinkedList<Ball> _ballsOnPaddle = new();

    public bool Pause { get; set; } = false;

    protected int blockCount = 0;

    public override void _Ready()
    {
        _paddle = GetNode<Paddle>("Paddle");
        _gameHud = GetNode<GameHud>("GameHud");
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

    protected void BoxDestroid()
    {
        blockCount--;
        Score++;
        _gameHud.SetScore(Score);
    }

    protected void AddBall()
    {
        var ball = BallScene.Instantiate<Ball>();
        BallsCount++;
        ball.Position = new Vector3(_paddle.Position.X, 0.5f, _paddle.Position.Z - 1.001f);

        _ballsOnPaddle.AddLast(ball);
        ball.BallLeavesScreen += BallLoose;
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

    protected void BallLoose()
    {
        BallsCount--;
        if (BallsCount == 0)
        {
            LifeLoose();
        }
    }

    protected void LifeLoose()
    {
        if (Lifes <= 0)
        {
            GameOver();
        }
        else
        {
            Lifes--;
            _gameHud.SetLifes(Lifes);
            AddBall();
        }
    }

    protected void GameOver()
    {
        _gameHud.GameOver();
    }
}
