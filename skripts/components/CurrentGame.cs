using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public class CurrentGameData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Dictionary<int, LevelProgressData> Levels { get; set; } = new();

    public UpgradeData Upgrades { get; set; } = new();

    public CurrentGameData()
    {
        for (int i = 1; i < GameScenePaths.LEVELS.Count; i++)
        {
            Levels[i] = new LevelProgressData(i, i == 1);
        }
    }

    public GameProgressData ToGameProgressData()
    {
        return new()
        {
            Id = Id,
            Levels = Levels.ToDictionary(e => e.Key, e => e.Value.Copy()),
            Upgrades = Upgrades.Copy(),
        };
    }
}

public class CurrentProgress
{
    public int Level { get; set; }
    public int Score { get; set; }
    public bool Ball1 { get; set; }
    public bool Ball2 { get; set; }
    public bool Ball3 { get; set; }
}

public class CurrentGame : GameComponet
{
    public CurrentGameData Game { get; private set; }

    public CurrentGame()
    {
        Game = new();
    }

    public void CreateNewGame()
    {
        Game = new();
        GameComponets.Instance.Get<GameProgress>().Save(Game.ToGameProgressData());
    }

    public void ResetGame()
    {
        Game = new();
    }

    public void LoadNewGame(GameProgressData data)
    {
        Game = new()
        {
            Id = data.Id
        };

        bool lastLvl = false;
        for (int i = 1; i < GameScenePaths.LEVELS.Count; i++)
        {
            if (!lastLvl && data.Levels.ContainsKey(i))
            {
                Game.Levels[i] = new LevelProgressData(data.Levels[i]);
            }
            else
            {
                lastLvl = true;
            }

            if (lastLvl)
            {
                Game.Levels[i] = new LevelProgressData(i);
            }
        }

        Game.Upgrades = data.Upgrades.Copy();
    }

    public void SaveProgress(CurrentProgress progress)
    {
        if (!Game.Levels.ContainsKey(progress.Level))
        {
            Debug.Print("Error saving. Data for level " + progress.Level + " cannot be saved.");
            return;
        }

        if (progress.Level > 1 && !Game.Levels.ContainsKey(progress.Level - 1) && !Game.Levels[progress.Level].Reached)
        {
            Debug.Print("Error saving. Level " + (progress.Level - 1) + " has not been completed yet.");
            return;
        }

        UpdateGameData(progress);

        GameComponets.Instance.Get<GameProgress>().Save(Game.ToGameProgressData());
    }

    private void UpdateGameData(CurrentProgress progress)
    {
        var currentData = Game.Levels[progress.Level];

        currentData.Ball1 = currentData.Ball1 || progress.Ball1;
        currentData.Ball2 = currentData.Ball2 || progress.Ball2;
        currentData.Ball3 = currentData.Ball3 || progress.Ball3;

        currentData.MaxScore = Math.Max(currentData.MaxScore, progress.Score);

        if (Game.Levels.ContainsKey(progress.Level + 1))
        {
            Game.Levels[progress.Level + 1].Reached = true;
        }
    }
}
