using System.Collections.Generic;
using Godot;

public abstract class AchievementFactory
{
    private readonly GodotObject scriptObject;

    protected AchievementFactory(GodotObject scriptObject)
    {
        this.scriptObject = scriptObject;
    }

    protected string Var(string variableName)
    {
        return scriptObject.Get(variableName).AsString();
    }

    protected string VarN(string variableName, int n)
    {
        return scriptObject.Call(variableName, n).AsString();
    }

    protected string VarN(string variableName, int n, int m)
    {
        return scriptObject.Call(variableName, n, m).AsString();
    }

    public abstract string GetAchievementText();
    public abstract IAchievementMonitor CreateMonitor();
}

public interface IAchievementMonitor
{
    public void GameSignal(AbstractSignal signal);
    public bool IsReached();
}

[GlobalClass]
public partial class Achievements : Node
{
    private GodotObject ScriptObject;

    private IDictionary<int, List<AchievementFactory>> ACHIEVEMENTS;

    public override void _Ready()
    {
        GDScript script = GD.Load<GDScript>("res://scenes/localization/Localization_Achievements.gd");
        ScriptObject = (GodotObject)script.New();

        ACHIEVEMENTS = new Dictionary<int, List<AchievementFactory>> {
            { 1, new List<AchievementFactory>(){ GET_X_POINTS(10),  CATCH_X_ROCKETS(1),  GET_X_ACTIVE_BALLS_AT_END(2)       }},
            { 2, new List<AchievementFactory>(){ GET_X_POINTS(100), GET_X_POINTS(150),   DONT_CATCH_ROCKETS()   }},
            { 3, new List<AchievementFactory>(){ GET_X_POINTS(110), GET_X_POINTS(150),   DONT_CATCH_ANY_ITEMS() }},
            { 4, new List<AchievementFactory>(){ GET_X_POINTS(100), GET_X_POINTS(140),   DONT_LOSE_ANY_LIFE()   }},
            { 5, new List<AchievementFactory>(){ GET_X_POINTS(100), GET_X_POINTS(140),   GET_X_POINTS(180)      }},
            { 6, new List<AchievementFactory>(){ GET_X_POINTS(100), GET_X_POINTS(180),   DONT_CATCH_ROCKETS()   }},
            { 7, new List<AchievementFactory>(){ GET_X_POINTS(200), GET_X_POINTS(400),   CATCH_X_BOMBS(4)       }},
            { 8, new List<AchievementFactory>(){ GET_X_POINTS(550), DONT_LOSE_ANY_LIFE(),DONT_CATCH_ROCKETS()   }},
            { 9, new List<AchievementFactory>(){ GET_X_POINTS(100), GET_X_POINTS(150),   DONT_LOSE_ANY_LIFE()   }},
            {10, new List<AchievementFactory>(){ GET_X_POINTS(160), GET_X_POINTS(200),   DONT_LOSE_ANY_LIFE()   }},
            {11, new List<AchievementFactory>(){ GET_X_POINTS(100), GET_X_POINTS(120),   DONT_LOSE_ANY_LIFE()   }},
            {12, new List<AchievementFactory>(){ GET_X_POINTS(150), GET_X_POINTS(200),   DONT_LOSE_ANY_LIFE()   }},
            {13, new List<AchievementFactory>(){ GET_X_POINTS(100), GET_X_POINTS(150),   DONT_LOSE_ANY_LIFE()   }},
            {14, new List<AchievementFactory>(){ GET_X_POINTS(150), GET_X_POINTS(200),   DONT_LOSE_ANY_LIFE()   }},
            {15, new List<AchievementFactory>(){ LEVEL_15_X_GOALS_K_SECONDS(2,4), LEVEL_15_X_GOALS_K_SECONDS(3,8),  DONT_LOSE_ANY_LIFE() }},
            {16, new List<AchievementFactory>(){ GET_X_POINTS(150), GET_X_POINTS(200),   DONT_LOSE_ANY_LIFE() }},
            {17, new List<AchievementFactory>(){ GET_X_POINTS(200), GET_X_POINTS(250),   DONT_LOSE_ANY_LIFE() }},
            {18, new List<AchievementFactory>(){ GET_X_POINTS(150), GET_X_POINTS(250),   CATCH_X_BOMBS(5) }},
            {19, new List<AchievementFactory>(){ GET_X_POINTS(300), GET_X_POINTS(400),   GET_X_ACTIVE_BALLS(4) }},
            {20, new List<AchievementFactory>(){ GET_X_POINTS(350), GET_X_ACTIVE_BALLS_AT_END(2), CATCH_X_ROCKETS(11) }},
            {21, new List<AchievementFactory>(){ GET_X_POINTS(350), GET_X_ACTIVE_BALLS_AT_END(7), CATCH_X_ROCKETS(19) }},
        };
    }

    private AchievementFactory GET_X_POINTS(int points)
    {
        return new GetXPointsAchievementFactory(ScriptObject, points);
    }

    private AchievementFactory CATCH_X_ROCKETS(int rockets)
    {
        return new CatchXRocketsAchievementFactory(ScriptObject, rockets);
    }

    private AchievementFactory CATCH_X_BOMBS(int bombs)
    {
        return new CatchXBombsAchievementFactory(ScriptObject, bombs);
    }

    private AchievementFactory DONT_CATCH_ROCKETS()
    {
        return new DontUseRocketsAchievementFactory(ScriptObject);
    }

    private AchievementFactory DONT_LOSE_ANY_LIFE()
    {
        return new DontLoseAnyLifeAchievementFactory(ScriptObject);
    }

    private AchievementFactory DONT_CATCH_ANY_ITEMS()
    {
        return new DontCatchAnyItemsAchievementFactory(ScriptObject);
    }

    private AchievementFactory LEVEL_15_X_GOALS_K_SECONDS(int count, int seconds)
    {
        return new Level15GoalAchievementFactory(ScriptObject, count, seconds);
    }

    private AchievementFactory GET_X_ACTIVE_BALLS(int count)
    {
        return new GetXActiveBallsAchievementFactory(ScriptObject, count);
    }

    private AchievementFactory GET_X_ACTIVE_BALLS_AT_END(int count)
    {
        return new GetXActiveBallsAtEndOfLevelAchievementFactory(ScriptObject, count);
    }

    public string GetAchivments(int level, int ball)
    {
        return ACHIEVEMENTS[level][ball].GetAchievementText();
    }

    public IAchievementMonitor GetMonitor(int level, int ball)
    {
        return ACHIEVEMENTS[level][ball].CreateMonitor();
    }
}
