using Godot;

public partial class GameHud : CanvasLayer
{
    private ScoreNumberLabel _scoreNumberLabel;

    private LifesNumberLabel _lifeNumberLabel;

    [Signal]
    public delegate void RestartLevelEventHandler();

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
        GetTree().ChangeSceneToPacked(menu);
    }

    private void OnRestartButtonPressed()
    {
        EmitSignal(SignalName.RestartLevel);
    }
}
