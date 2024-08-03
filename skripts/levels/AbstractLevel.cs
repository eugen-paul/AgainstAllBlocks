using Godot;

public partial class AbstractLevel : Node
{
    [Export]
    public int LIFES_COUNT { get; set; } = 3;

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

    private bool _running;

    public int BlockCount { get; private set; }

    public override void _Ready()
    {
        _paddle = GetNode<Paddle>("Paddle");
        _gameHud = GetNode<GameHud>("GameHud");
        _gameHud.RestartLevel += Restart;
        _gameHud.PauseEvent += PauseEvent;

        GetNode<CpuParticles3D>("Explosion").Finished += StartRound;
        StartRound();

        Lifes = LIFES_COUNT;
        _score = 0;
        BlockCount = 0;
        _gameHud.StartGame();
        _gameHud.SetScore(_score);
        _gameHud.SetLifes(Lifes);

        BlockCount = CountBlocks();

        var prefs = GameComponets.Instance.Get<UserPreferences>();
        if (!prefs.GetParamShowBackground())
        {
            var bg = GetNode<Node>("Background");
            bg.QueueFree();
        }
    }

    /// <summary>
    /// Counts all undestroyed blocks on the field. The objects with the name <c>Block*</c> are counted.
    /// </summary>
    /// <returns>Number of blocks on the field</returns>
    protected virtual int CountBlocks()
    {
        var blocks = FindChildren("Block*");
        var count = 0;
        foreach (var nodeBlock in blocks)
        {
            if (nodeBlock is Block block)
            {
                block.BlockDestroyed += BoxDestroid;
                count++;
            }
        }
        return count;
    }

    private void Restart()
    {
        GetTree().Paused = false;
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
        if (@event.IsActionPressed("shoot") && _startBall != null)
        {
            _startBall.Velocity = Vector3.Forward.Rotated(Vector3.Up, _startArrow.Rotation.Y) * _startBall.StartSpeed;
            _startBall = null;
            _startArrow.QueueFree();
        }
    }

    private void PauseEvent()
    {
        if (!_running)
        {
            return;
        }

        if (GetTree().Paused)
        {
            _gameHud.HidePauseScreen();
            GetTree().Paused = false;
        }
        else
        {
            _gameHud.ShowPauseScreen();
            GetTree().Paused = true;
        }
    }

    protected void BallLoose()
    {
        BallsCount--;
        if (BallsCount == 0 && BlockCount > 0)
        {
            Lifes--;
            _gameHud.SetLifes(Lifes);

            if (Lifes <= 0)
            {
                GameOver();
            }
            else
            {
                LifeLoose();
            }
        }
    }

    protected void LifeLoose()
    {
        GetNode<CpuParticles3D>("Explosion").Show();
        GetNode<CpuParticles3D>("Explosion").Position = _paddle.Position;
        GetNode<CpuParticles3D>("Explosion").Restart();
        GetNode<CpuParticles3D>("Explosion").Finished += StartRound;
        _paddle.Hide();
    }

    protected void StartRound()
    {
        _running = true;

        GetNode<CpuParticles3D>("Explosion").Hide();
        _paddle.Show();

        GetNode<CpuParticles3D>("Explosion").Finished -= StartRound;

        AddStartBall();
    }

    protected void GameOver()
    {
        GetNode<CpuParticles3D>("Explosion").Show();
        GetNode<CpuParticles3D>("Explosion").Position = _paddle.Position;
        GetNode<CpuParticles3D>("Explosion").Restart();
        _paddle.Hide();
        _running = false;
        _gameHud.GameOver();
    }

    protected void LevelDone()
    {
        _running = false;
        _gameHud.LevelDone();
    }
}
