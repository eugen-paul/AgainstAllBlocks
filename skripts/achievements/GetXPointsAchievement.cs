
using Godot;

public class GetXPointsAchievementFactory : AchievementFactory
{
    private readonly int minPoints;

    public GetXPointsAchievementFactory(GodotObject scriptObject, int minPoints) : base(scriptObject)
    {
        this.minPoints = minPoints;
    }

    public override IAchievementMonitor CreateMonitor()
    {
        return new GetXPointsAchievement(minPoints);
    }

    public override string GetAchievementText()
    {
        return string.Format(Var("GET_X_POINTS"), minPoints);
    }
}

public class GetXPointsAchievement : IAchievementMonitor
{
    private int score = 0;
    private readonly int minScore = 0;

    public GetXPointsAchievement(int minScore)
    {
        this.minScore = minScore;
    }

    public void GameSignal(AbstractSignal signal)
    {
        if (signal is ScoreChangeSignal s)
        {
            score = s.Score;
        }
    }

    public bool IsReached()
    {
        return score >= minScore;
    }
}