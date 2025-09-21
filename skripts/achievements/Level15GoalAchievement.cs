
using System;
using System.Collections.Generic;
using Godot;

public class Level15GoalAchievementFactory : AchievementFactory
{
    private readonly int seconds;
    private readonly int count;

    public Level15GoalAchievementFactory(GodotObject scriptObject, int count, int seconds) : base(scriptObject)
    {
        this.count = count;
        this.seconds = seconds;
    }

    public override IAchievementMonitor CreateMonitor()
    {
        return new Level15GoalAchievement(count, seconds);
    }

    public override string GetAchievementText()
    {
        return string.Format(VarN("FUNC_LEVEL_15_X_GOALS_K_SECONDS", count, seconds), count, seconds);
    }
}

public class Level15GoalAchievement : IAchievementMonitor
{
    private readonly List<long> goalsTime = [];
    private readonly int seconds;
    private readonly int count;

    public Level15GoalAchievement(int count, int seconds)
    {
        this.count = count;
        this.seconds = seconds;
    }

    public void GameSignal(AbstractSignal signal)
    {
        if (signal is GoalSignal)
        {
            goalsTime.Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }
    }

    public bool IsReached()
    {
        if (goalsTime.Count < count)
        {
            return false;
        }

        for (int i = 0; i <= goalsTime.Count - count; i++)
        {
            if (goalsTime[i + count - 1] - goalsTime[i] <= seconds * 1000)
            {
                return true;
            }
        }
        return false;
    }
}