using System.Diagnostics;
using Godot;

public partial class GameHud : CanvasLayer
{
    private ScoreNumberLabel _scoreNumberLabel;

    private LifesNumberLabel _lifeNumberLabel;

    [Signal]
    public delegate void RestartLevelEventHandler();

    [Signal]
    public delegate void PauseEventEventHandler();

    public override void _Ready()
    {
        _scoreNumberLabel = GetNode<ScoreNumberLabel>("ScoreHBoxContainer/ScoreNumberLabel");
        _lifeNumberLabel = GetNode<LifesNumberLabel>("LifesHBoxContainer/LifesNumberLabel");
        StartGame();
    }

    public void SetScore(int score)
    {
        _scoreNumberLabel.SetScore(score);
    }

    public void SetLifes(int lifes)
    {
        _lifeNumberLabel.SetLifes(lifes);
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

    private void OnMenuButtonPressed()
    {
        var menu = ResourceLoader.Load<PackedScene>("res://menu/Main.tscn");
        GetTree().Paused = false;
        GetTree().ChangeSceneToPacked(menu);
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
            EmitSignal(SignalName.PauseEvent);
        }
    }
}
