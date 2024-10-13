using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using Godot.Collections;

public abstract partial class AbstractLevel : Node
{
    [Export]
    public int LIFES_COUNT { get; set; } = 3;

    [Export]
    public PackedScene BallScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_BALL_SCENE);

    [Export]
    public PackedScene ArrowScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_ARROW_SCENE);

    public int Lifes { get; private set; }

    private int score = 0;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            gameHud.SetScore(Score);
        }
    }

    private HashSet<Node3D> temporaryObjects;
    private HashSet<Ball> balls;

    protected Paddle paddle;
    protected GameHud gameHud;

    protected Ball startBall;
    protected Arrow startArrow;

    private bool running;

    public override void _Ready()
    {
        temporaryObjects = new();
        balls = new();

        paddle = GetNode<Paddle>("Paddle");

        Lifes = LIFES_COUNT;

        gameHud = GetNode<GameHud>("GameHud");
        gameHud.RestartLevel += Restart;
        gameHud.PauseEvent += PauseEvent;
        gameHud.CurrentLevel = GetLevel();
        gameHud.StartGame();
        gameHud.SetScore(Score);
        gameHud.SetLifes(Lifes);

        GetNode<GpuParticles3D>("Explosion").Finished += StartRound;
        StartRound();

        InitBlockDestoyedCallbacks();

        InitStages();
        SetBackground();
        // SetLights();
    }

    private void SetBackground()
    {
        var prefs = GameComponets.Instance.Get<UserPreferences>();
        if (!prefs.GetParamShowBackground())
        {
            var bg = GetNode<Node>("Background");
            bg.QueueFree();
        }
    }

    private void SetLights()
    {
        var prefs = GameComponets.Instance.Get<UserPreferences>();
        var lights = GetNodeOrNull<Node>("Lights");
        if (lights == null)
        {
            return;
        }

        GD.Print("lights.GetChildren.count: " + lights.GetChildren().Count);
        foreach (var light in lights.GetChildren())
        {
            GD.Print("edit object: " + light.GetType());
            if (light is Light3D light3D)
            {
                GD.Print("edit Light");
                light3D.Visible = prefs.GetParamShowShadow();
                light3D.ShadowEnabled = prefs.GetParamShowShadow();
            }
            else if (light is WorldEnvironment we)
            {
                GD.Print("edit we");
                if (prefs.GetParamShowShadow())
                {
                    GD.Print("set to black");
                    we.Environment.AmbientLightEnergy = 0.4f;
                }
                else
                {
                    GD.Print("set to white");
                    we.Environment.AmbientLightEnergy = 1.4f;
                }
            }
            else
            {
                GD.Print("type = " + light.GetType());
            }
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
            if (nodeBlock is ABlock block)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Counts all undestroyed blocks on the field. The objects with the name <c>Block*</c> are counted.
    /// </summary>
    /// <returns>Number of blocks on the field</returns>
    protected virtual void InitBlockDestoyedCallbacks()
    {
        var blocks = FindChildren("Block*");
        foreach (var nodeBlock in blocks)
        {
            if (nodeBlock is ABlock block)
            {
                block.BlockDestroyed += BlockDestroid;
            }
        }
    }

    protected virtual void InitStages()
    {
        var stages = FindChildren("Stage*");
        foreach (var nodeStage in stages)
        {
            if (nodeStage is StageSquare stage)
            {
                stage.Level = this;
            }
        }
    }

    public virtual Array<Node> GetBlocks()
    {
        return FindChildren("Block*");
    }

    protected abstract int GetLevel();
    protected abstract bool GetBall1();
    protected abstract bool GetBall2();
    protected abstract bool GetBall3();

    private void Restart()
    {
        GetTree().Paused = false;
        GetTree().CallDeferred(SceneTree.MethodName.ReloadCurrentScene);
    }

    protected void BlockDestroid(int scoreBonus, ContainItem itemToCreate)
    {
        Score = Mathf.Max(0, Score + scoreBonus + 1);
        if (CountBlocks() <= 1) // "<= 1", because the function is called first and only then is the block deleted.
        {
            LevelDone();
        }
        else
        {
            var item = itemToCreate.CreateItem(this);
            if (item != null)
            {
                temporaryObjects.Add(item);
                AddChild(item);
            }
        }
    }

    public void TemporaryAdd(Node3D obj)
    {
        temporaryObjects.Add(obj);
    }

    public void TemporaryDestroyd(Node3D obj)
    {
        temporaryObjects.Remove(obj);
    }

    public Paddle GetPaddle()
    {
        return paddle;
    }

    protected void AddStartBall()
    {
        startArrow = ArrowScene.Instantiate<Arrow>();
        startArrow.Position = new Vector3(0, 0.5f, paddle.Position.Z - 2f);
        AddChild(startArrow);

        startBall = BallScene.Instantiate<Ball>();
        startBall.Position = startArrow.Position;
        startBall.BallLeavesScreen += BallLoose;
        balls.Add(startBall);
        AddChild(startBall);
    }

    public void AddBall(float x)
    {
        var ball = BallScene.Instantiate<Ball>();
        ball.Position = new Vector3(x, 0.5f, paddle.Position.Z - 1f);
        ball.BallLeavesScreen += BallLoose;
        ball.Velocity = Vector3.Back * ball.StartSpeed;
        balls.Add(ball);
        AddChild(ball);
    }

    public HashSet<Ball> GetAllBalls()
    {
        return balls;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("shoot") && startBall != null)
        {
            startBall.Velocity = Vector3.Forward.Rotated(Vector3.Up, startArrow.Rotation.Y) * startBall.StartSpeed;
            startBall.PlaySound(BallSounds.KICK);
            startBall = null;
            startArrow.QueueFree();
        }
    }

    private void PauseEvent()
    {
        if (!running)
        {
            return;
        }

        if (GetTree().Paused)
        {
            gameHud.HidePauseScreen();
            GetTree().Paused = false;
        }
        else
        {
            gameHud.ShowPauseScreen();
            GetTree().Paused = true;
        }
    }

    protected void BallLoose(Ball ball)
    {
        balls.Remove(ball);
        if (balls.Count == 0)
        {
            Lifes--;
            if (Lifes <= 0)
            {
                GameOver();
            }
            else
            {
                paddle.PlayLoseLife();
                LifeLoose();
            }
        }
        else
        {
            paddle.PlayLoseBall();
        }
    }

    public void LifesAdd(int value)
    {
        Lifes += value;
        gameHud.SetLifes(Lifes);
        if (Lifes <= 0)
        {
            GameOver();
        }
    }

    public void Death()
    {
        Lifes--;
        gameHud.SetLifes(Lifes);
        if (Lifes <= 0)
        {
            GameOver();
        }
        else
        {
            LifeLoose();
        }
    }

    protected void LifeLoose()
    {
        DestroyAllTemporary();
        GetNode<GpuParticles3D>("Explosion").Show();
        GetNode<GpuParticles3D>("Explosion").Position = paddle.Position;
        GetNode<GpuParticles3D>("Explosion").Restart();
        GetNode<GpuParticles3D>("Explosion").Finished += StartRound;
        gameHud.SetLifes(Lifes);
        paddle.Hide();
    }

    protected void StartRound()
    {
        running = true;

        GetNode<GpuParticles3D>("Explosion").Hide();
        paddle.Show();
        paddle.Reset();

        GetNode<GpuParticles3D>("Explosion").Finished -= StartRound;

        AddStartBall();
    }

    protected void GameOver()
    {
        DestroyAllTemporary();
        GetNode<GpuParticles3D>("Explosion").Show();
        GetNode<GpuParticles3D>("Explosion").Position = paddle.Position;
        GetNode<GpuParticles3D>("Explosion").Restart();
        paddle.Hide();
        running = false;
        gameHud.GameOver();
    }

    private void DestroyAllTemporary()
    {
        foreach (var obj in temporaryObjects)
        {
            obj.QueueFree();
        }
        temporaryObjects.Clear();

        foreach (var ball in balls)
        {
            ball.QueueFree();
        }
        balls.Clear();
    }

    /// <summary>
    /// Level has been successfully completed. Score is being updated.
    /// </summary>
    protected void LevelDone()
    {
        DestroyAllTemporary();
        running = false;
        SaveProgress();
        gameHud.LevelDone();
    }

    private void SaveProgress()
    {
        var progress = new CurrentProgress
        {
            Level = GetLevel(),
            Score = Score,
            Ball1 = GetBall1(),
            Ball2 = GetBall2(),
            Ball3 = GetBall3(),
        };
        GameComponets.Instance.Get<CurrentGame>().SaveProgress(progress);
    }
}
