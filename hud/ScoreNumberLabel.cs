using Godot;

public partial class ScoreNumberLabel : Label
{
    public void SetScore(int score)
    {
        Text = score.ToString();
    }
}
