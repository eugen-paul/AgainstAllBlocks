
using Godot;

public class DontLoseAnyLifeAchievementFactory : AchievementFactory
{
    public DontLoseAnyLifeAchievementFactory(GodotObject scriptObject) : base(scriptObject)
    {
    }

    public override IAchievementMonitor CreateMonitor()
    {
        return new DontLoseAnyLifeAchievement();
    }

    public override string GetAchievementText()
    {
        return string.Format(Var("DONT_LOSE_ANY_LIFE"));
    }
}

public class DontLoseAnyLifeAchievement : IAchievementMonitor
{
    private bool done = true;

    public void GameSignal(AbstractSignal signal)
    {
        if (signal is LoseLifeSignal)
        {
            done = false;
        }
    }

    public bool IsReached()
    {
        return done;
    }
}