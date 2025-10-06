
using Godot;

public class GetXActiveBallsAtEndOfLevelAchievementFactory : AchievementFactory
{
    private readonly int countToReach;

    public GetXActiveBallsAtEndOfLevelAchievementFactory(GodotObject scriptObject, int countToReach) : base(scriptObject)
    {
        this.countToReach = countToReach;
    }

    public override IAchievementMonitor CreateMonitor()
    {
        return new GetXActiveBallsAtEndOfLevelAchievement(countToReach);
    }

    public override string GetAchievementText()
    {
        return string.Format(VarN("FUNC_GET_X_ACTIVE_BALLS_AT_END", countToReach), countToReach);
    }
}

public class GetXActiveBallsAtEndOfLevelAchievement : IAchievementMonitor
{
    private int count;
    private readonly int countToReach;

    public GetXActiveBallsAtEndOfLevelAchievement(int countToReach)
    {
        this.count = 0;
        this.countToReach = countToReach;
    }

    public void GameSignal(AbstractSignal signal)
    {
        if (signal is LoseBallSignal)
        {
            count--;
        }
        else if (signal is NewBallSignal)
        {
            count++;
        }
    }

    public bool IsReached()
    {
        return count >= countToReach;
    }
}