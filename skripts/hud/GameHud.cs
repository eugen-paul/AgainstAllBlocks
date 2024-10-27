using Godot;

public partial class GameHud : CanvasLayer
{

    public int CurrentLevel { get; set; }

    private ScoreNumberLabel scoreNumberLabel;

    private LifeContainer lifeContainer;

    [Signal]
    public delegate void RestartLevelEventHandler();

    [Signal]
    public delegate void PauseEventEventHandler();

    public override void _Ready()
    {
        scoreNumberLabel = GetNode<ScoreNumberLabel>("TopPanel/ScoreNumberLabel");
        lifeContainer = GetNode<LifeContainer>("TopPanel/LifesHBoxContainer");
        var prefs = GameComponets.Instance.Get<UserPreferences>();
        if (prefs.GetParamShowFps())
        {
            GetNode<CanvasLayer>("Fps").Show();
        }
        if (CurrentLevel >= GameScenePaths.MaxLevel)
        {
            GetNode<Button>("WinRect/CenterContainer/VBoxContainer/VBoxContainer/NextGameButton").Hide();
        }
        StartGame();
    }

    public void SetScore(int score)
    {
        scoreNumberLabel.SetScore(score);
    }

    public void SetLifes(int lifes)
    {
        lifeContainer.SetLifes(lifes);
    }

    public void StartGame()
    {
        GetNode<Control>("GameOverRect").Hide();
        GetNode<Control>("WinRect").Hide();
        GetNode<Control>("PauseRect").Hide();
    }

    public void GameOver()
    {
        GetNode<AudioStreamPlayer>("GameOverRect/LosePlayer").Playing = true;
        GetNode<Control>("GameOverRect").Show();
        GetNode<AnimationPlayer>("GameOverRect/AnimationPlayer").Play("show");
    }

    public void LevelDone(GoldenBallStatus.BallStatus[] ballStatus)
    {
        GetNode<Control>("WinRect").Show();
        GetNode<AnimationPlayer>("WinRect/AnimationPlayer").Play("show");

        var Achievements = GetNode<Achievements>("Achievements");
        GetNode<GoldenBallStatus>("WinRect/CenterContainer/VBoxContainer/GoldenBallStatus").ShowBalls(ballStatus[0], ballStatus[1], ballStatus[2]);
        GetNode<GoldenBallStatus>("WinRect/CenterContainer/VBoxContainer/GoldenBallStatus").SetTooltips(
            Achievements.GetAchivments(CurrentLevel, 0),
            Achievements.GetAchivments(CurrentLevel, 1),
            Achievements.GetAchivments(CurrentLevel, 2)
        );
        LevelDoneAnimation();
    }

    private void LevelDoneAnimation()
    {
        GetNode<AudioStreamPlayer>("WinRect/CenterContainer/WinPlayer").Playing = true;
        GetTree().CreateTimer(0.5, false, true).Timeout += () => ShowAnimation("WinRect/CenterContainer/Firework1");
        GetTree().CreateTimer(1.5, false, true).Timeout += () => ShowAnimation("WinRect/CenterContainer/Firework2");
        GetTree().CreateTimer(2.5, false, true).Timeout += () => ShowAnimation("WinRect/CenterContainer/Firework3");

        // GetNode<CpuParticles2D>("WinRect/CenterContainer/VBoxContainer/CPUParticles2D1").Show();
        // GetNode<CpuParticles2D>("WinRect/CenterContainer/VBoxContainer/CPUParticles2D1").Restart();
        // await ToSignal(GetTree().CreateTimer(1.0, false, true), SceneTreeTimer.SignalName.Timeout);

        // GetNode<CpuParticles2D>("WinRect/CenterContainer/VBoxContainer/CPUParticles2D2").Show();
        // GetNode<CpuParticles2D>("WinRect/CenterContainer/VBoxContainer/CPUParticles2D2").Restart();
        // await ToSignal(GetTree().CreateTimer(1.0, false, true), SceneTreeTimer.SignalName.Timeout);

        // GetNode<CpuParticles2D>("WinRect/CenterContainer/VBoxContainer/CPUParticles2D3").Show();
        // GetNode<CpuParticles2D>("WinRect/CenterContainer/VBoxContainer/CPUParticles2D3").Restart();
    }

    private void ShowAnimation(string animationName)
    {
        var firework = GetNode<Firework>(animationName);
        firework.Show();
        firework.Fire();
    }

    private void OnNextLevelButtonPressed()
    {
        var next = ResourceLoader.Load<PackedScene>(GameScenePaths.GetLevelPath(CurrentLevel + 1));
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
    }

    private void OnMenuButtonPressed()
    {
        var next = ResourceLoader.Load<PackedScene>(GameScenePaths.LEVEL_SELECTION_SCENE);
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
        GetTree().Paused = false;
    }

    private void OnPauseButtonPressed()
    {
        EmitSignal(SignalName.PauseEvent);
    }

    private void OnContinueButtonPressed()
    {
        EmitSignal(SignalName.PauseEvent);
    }

    private void OnRestartButtonPressed()
    {
        EmitSignal(SignalName.RestartLevel);
    }

    public void ShowPauseScreen()
    {
        GetNode<Control>("PauseRect").Show();
    }

    public void HidePauseScreen()
    {
        GetNode<Control>("PauseRect").Hide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            OnPauseButtonPressed();
        }
    }

    public override void _Notification(int what)
    {
        switch ((long)what)
        {
            case NotificationWMGoBackRequest:
                OnPauseButtonPressed();
                break;
        }
    }
}
