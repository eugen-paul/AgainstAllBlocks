
using Godot;

public class GetXActiveBallsAchievementFactory : AchievementFactory
{
    private readonly int countToReach;

    public GetXActiveBallsAchievementFactory(GodotObject scriptObject, int countToReach) : base(scriptObject)
    {
        this.countToReach = countToReach;
    }

    public override IAchievementMonitor CreateMonitor()
    {
        return new GetXActiveBallsAchievement(countToReach);
    }

    public override string GetAchievementText()
    {
        return string.Format(VarN("FUNC_GET_X_ACTIVE_BALLS", countToReach), countToReach);
    }
}

public class GetXActiveBallsAchievement : IAchievementMonitor
{
    private int count;
    private readonly int countToReach;

    public GetXActiveBallsAchievement(int countToReach)
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