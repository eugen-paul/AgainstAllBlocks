
using Godot;

public class CatchXRocketsAchievementFactory : AchievementFactory
{
    private readonly int minRockets;

    public CatchXRocketsAchievementFactory(GodotObject scriptObject, int minRockets) : base(scriptObject)
    {
        this.minRockets = minRockets;
    }

    public override IAchievementMonitor CreateMonitor()
    {
        return new CatchXRocketsAchievement(minRockets);
    }

    public override string GetAchievementText()
    {
        return string.Format(VarN("FUNC_CATCH_X_ROCKETS", minRockets), minRockets);
    }
}

public class CatchXRocketsAchievement : IAchievementMonitor
{
    private int rockets = 0;
    private readonly int minRockets = 0;

    public CatchXRocketsAchievement(int minRockets)
    {
        this.minRockets = minRockets;
    }

    public void GameSignal(AbstractSignal signal)
    {
        if (signal is ItemCatchSignal s && s.Type == ItemType.ROCKET)
        {
            rockets++;
        }
    }

    public bool IsReached()
    {
        return rockets >= minRockets;
    }
}