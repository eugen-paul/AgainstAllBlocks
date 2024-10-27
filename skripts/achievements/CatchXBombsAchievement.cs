
using Godot;

public class CatchXBombsAchievementFactory : AchievementFactory
{
    private readonly int minBombs;

    public CatchXBombsAchievementFactory(GodotObject scriptObject, int minBombs) : base(scriptObject)
    {
        this.minBombs = minBombs;
    }

    public override IAchievementMonitor CreateMonitor()
    {
        return new CatchXBombsAchievement(minBombs);
    }

    public override string GetAchievementText()
    {
        return string.Format(Var("CATCH_X_BOMBS"), minBombs);
    }
}

public class CatchXBombsAchievement : IAchievementMonitor
{
    private int bombs = 0;
    private readonly int minBombs = 0;

    public CatchXBombsAchievement(int minBombs)
    {
        this.minBombs = minBombs;
    }

    public void GameSignal(AbstractSignal signal)
    {
        if (signal is ItemCatchSignal s && s.Type == ItemType.BOMBS)
        {
            bombs++;
        }
    }

    public bool IsReached()
    {
        return bombs >= minBombs;
    }
}