using Godot;

public partial class GameHud : CanvasLayer
{
    [Export]
    private PackedScene HeartAnimPath { get; set; }

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
    }

    private void OnMainMenuButtonPressed()
    {
        var menu = ResourceLoader.Load<PackedScene>(GamePaths.MAIN_SCENE);
        GetTree().Paused = false;
        GetTree().ChangeSceneToPacked(menu);
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
