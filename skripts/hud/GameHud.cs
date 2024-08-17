using System.Diagnostics;
using Godot;

public partial class GameHud : CanvasLayer
{
    [Export]
    private PackedScene HeartAnimPath { get; set; }

    public int CurrentLevel { get; set; }

    private ScoreNumberLabel _scoreNumberLabel;

    private HBoxContainer _lifeContainer;

    [Signal]
    public delegate void RestartLevelEventHandler();

    [Signal]
    public delegate void PauseEventEventHandler();

    public override void _Ready()
    {
        _scoreNumberLabel = GetNode<ScoreNumberLabel>("TopPanel/ScoreNumberLabel");
        _lifeContainer = GetNode<HBoxContainer>("TopPanel/LifesHBoxContainer");
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
        _scoreNumberLabel.SetScore(score);
    }

    public void SetLifes(int lifes)
    {
        if (lifes < 0)
        {
            return;
        }

        var count = _lifeContainer.GetChildCount();
        if (lifes > count)
        {
            for (int i = count; i < lifes; i++)
            {
                Heart heart = HeartAnimPath.Instantiate<Heart>();
                _lifeContainer.AddChild(heart);
                heart.PlayCreate();
            }
        }
        else if (lifes < count)
        {
            for (int i = count; i > lifes; i--)
            {
                Heart heart = (Heart)_lifeContainer.GetChild(0);
                heart.PlayDestroy(() => _lifeContainer.RemoveChild(_lifeContainer.GetChild(0)));
            }
        }
    }

    public void StartGame()
    {
        GetNode<Control>("GameOverRect").Hide();
        GetNode<Control>("WinRect").Hide();
        GetNode<Control>("PauseRect").Hide();
    }

    public void GameOver()
    {
        GetNode<Control>("GameOverRect").Show();
        GetNode<AnimationPlayer>("GameOverRect/AnimationPlayer").Play("show");
    }

    public void LevelDone()
    {
        GetNode<Control>("WinRect").Show();
        GetNode<AnimationPlayer>("WinRect/AnimationPlayer").Play("show");
        LevelDoneAnimation();
    }

    private void LevelDoneAnimation()
    {
        ShowAnimation("WinRect/CenterContainer/VBoxContainer/CPUParticles2D1");
        GetTree().CreateTimer(1.0, false, true).Timeout += () => ShowAnimation("WinRect/CenterContainer/VBoxContainer/CPUParticles2D2");
        GetTree().CreateTimer(2.0, false, true).Timeout += () => ShowAnimation("WinRect/CenterContainer/VBoxContainer/CPUParticles2D3");

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
        GetNode<CpuParticles2D>(animationName).Show();
        GetNode<CpuParticles2D>(animationName).Restart();
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
