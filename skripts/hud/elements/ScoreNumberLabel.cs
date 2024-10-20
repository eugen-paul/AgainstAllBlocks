using Godot;

public partial class ScoreNumberLabel : Control
{
    public void SetScore(int score)
    {
        GetNode<Label>("Outline").Text = score.ToString();
        GetNode<Label>("ScoreNumberLabel").Text = score.ToString();
    }
}
