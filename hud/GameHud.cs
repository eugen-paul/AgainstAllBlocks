using Godot;

public partial class GameHud : CanvasLayer
{
    private ScoreNumberLabel _scoreNumberLabel;

    private LifesNumberLabel _lifeNumberLabel;

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
    }

    public void GameOver()
    {
        GetNode<Control>("GameOverRect").Show();
        GetNode<AnimationPlayer>("GameOverRect/AnimationPlayer").Play("show");
    }
}
