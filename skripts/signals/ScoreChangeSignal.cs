public class ScoreChangeSignal : AbstractSignal
{
    public int Score { get; }

    public ScoreChangeSignal(int Score)
    {
        this.Score = Score;
    }
}