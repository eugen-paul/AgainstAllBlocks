
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
        return VarN("FUNC_LEVEL_15_X_GOALS_K_SECONDS", count, seconds);
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
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            goalsTime.Add(now);
            while (goalsTime.Count > 0 && goalsTime[0] < now - seconds * 1000)
            {
                goalsTime.RemoveAt(0);
            }
        }
    }

    public bool IsReached()
    {
        return goalsTime.Count >= count;
    }
}