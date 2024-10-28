
using Godot;

public class DontCatchAnyItemsAchievementFactory : AchievementFactory
{
    public DontCatchAnyItemsAchievementFactory(GodotObject scriptObject) : base(scriptObject)
    {
    }

    public override IAchievementMonitor CreateMonitor()
    {
        return new DontCatchAnyItemsAchievement();
    }

    public override string GetAchievementText()
    {
        return string.Format(Var("DONT_CATCH_ANY_ITEMS"));
    }
}

public class DontCatchAnyItemsAchievement : IAchievementMonitor
{
    private bool done = true;

    public void GameSignal(AbstractSignal signal)
    {
        if (signal is ItemCatchSignal)
        {
            done = false;
        }
    }

    public bool IsReached()
    {
        return done;
    }
}