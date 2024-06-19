using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Godot;

public partial class AbstractLevel : Node
{
    [Export]
    public PackedScene BallScene { get; set; }

    [Export]
    public PackedScene ArrowScene { get; set; }

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

    protected Ball _startBall;
    protected Arrow _startArrow;

    public bool Pause { get; set; } = false;

    protected int blockCount = 0;

    public override void _Ready()
    {
        _paddle = GetNode<Paddle>("Paddle");
        _gameHud = GetNode<GameHud>("GameHud");

        GetNode<CpuParticles3D>("Explosion").Finished += StartRound;
        StartRound();

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

    protected void AddStartBall()
    {
        _startArrow = ArrowScene.Instantiate<Arrow>();
        _startArrow.Position = new Vector3(0, 0.5f, _paddle.Position.Z - 2f);
        AddChild(_startArrow);

        _startBall = BallScene.Instantiate<Ball>();
        BallsCount++;
        _startBall.Position = _startArrow.Position;
        _startBall.BallLeavesScreen += BallLoose;
        AddChild(_startBall);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("shoot") && !Pause && _startBall != null)
        {
            _startBall.Velocity = Vector3.Forward.Rotated(Vector3.Up, _startArrow.Rotation.Y) * _startBall.Speed;
            _startArrow.QueueFree();
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
            GetNode<CpuParticles3D>("Explosion").Show();
            GetNode<CpuParticles3D>("Explosion").Position = _paddle.Position;
            GetNode<CpuParticles3D>("Explosion").Restart();
            _paddle.Hide();
            GameOver();
        }
        else
        {
            Lifes--;
            _gameHud.SetLifes(Lifes);

            GetNode<CpuParticles3D>("Explosion").Show();
            GetNode<CpuParticles3D>("Explosion").Position = _paddle.Position;
            GetNode<CpuParticles3D>("Explosion").Restart();
            GetNode<CpuParticles3D>("Explosion").Finished += StartRound;
            _paddle.Hide();
        }
    }
    protected void StartRound()
    {
        GetNode<CpuParticles3D>("Explosion").Hide();
        _paddle.Show();

        GetNode<CpuParticles3D>("Explosion").Finished -= StartRound;

        AddStartBall();
    }

    protected void GameOver()
    {
        _gameHud.GameOver();
    }
}
