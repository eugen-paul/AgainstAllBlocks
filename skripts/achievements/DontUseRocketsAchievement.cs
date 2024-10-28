
using Godot;

public class DontUseRocketsAchievementFactory : AchievementFactory
{
    public DontUseRocketsAchievementFactory(GodotObject scriptObject) : base(scriptObject)
    {
    }

    public override IAchievementMonitor CreateMonitor()
    {
        return new DontUseRocketsAchievement();
    }

    public override string GetAchievementText()
    {
        return string.Format(Var("DONT_USE_ROCKETS"));
    }
}

public class DontUseRocketsAchievement : IAchievementMonitor
{
    private bool done = true;

    public void GameSignal(AbstractSignal signal)
    {
        if (signal is ItemCatchSignal s && s.Type == ItemType.ROCKET)
        {
            done = false;
        }
    }

    public bool IsReached()
    {
        return done;
    }
}