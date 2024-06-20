using Godot;

public partial class AbstractLevel : Node
{
    [Export]
    public int LIFES_COUNT { get; set; } = 5;

    [Export]
    public PackedScene BallScene { get; set; }

    [Export]
    public PackedScene ArrowScene { get; set; }

    public int Lifes { get; private set; }

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

    public int BlockCount { get; private set; }

    public override void _Ready()
    {
        _paddle = GetNode<Paddle>("Paddle");
        _gameHud = GetNode<GameHud>("GameHud");
        _gameHud.RestartLevel += Restart;

        GetNode<CpuParticles3D>("Explosion").Finished += StartRound;
        StartRound();

        Lifes = LIFES_COUNT;
        BlockCount = 0;
        _gameHud.StartGame();
        _gameHud.SetScore(0);
        _gameHud.SetLifes(0);

        var blocks = FindChildren("Block*");
        foreach (var nodeBlock in blocks)
        {
            if (nodeBlock is Block block)
            {
                block.BlockDestroyed += BoxDestroid;
                BlockCount++;
            }
        }
    }

    private void Restart()
    {
        GetTree().ReloadCurrentScene();
    }

    protected void BoxDestroid(int scoreBonus)
    {
        BlockCount--;
        Score += scoreBonus + 1;
        _gameHud.SetScore(Score);

        if (BlockCount <= 0)
        {
            LevelDone();
        }
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
            _startBall = null;
            _startArrow.QueueFree();
        }
    }

    protected void BallLoose()
    {
        BallsCount--;
        if (BallsCount == 0 && BlockCount > 0)
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

    protected void LevelDone()
    {
        _gameHud.LevelDone();
    }
}
