using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

public class CurrentGameDataBuilder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public IDictionary<int, LevelProgressData> Levels { get; set; } = new Dictionary<int, LevelProgressData>();
    public UpgradeData Upgrades { get; set; } = new();
}

public class CurrentGameData
{
    public Guid Id { get; private set; }
    public IDictionary<int, LevelProgressData> Levels { get; set; }

    public UpgradeData Upgrades { get; set; }

    public UpgradeController UpgradeController { get; private set; } = new();

    public CurrentGameData(CurrentGameDataBuilder builder)
    {
        Id = builder.Id;
        Levels = new Dictionary<int, LevelProgressData>();
        for (int i = 1; i < GameScenePaths.LEVELS.Count; i++)
        {
            if (builder.Levels.ContainsKey(i))
            {
                Levels[i] = builder.Levels[i];
            }
            else
            {
                Levels[i] = new LevelProgressData(i, i == 1);
            }
        }
        Upgrades = builder.Upgrades;
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

    public bool UpdateGameData(CurrentProgress progress)
    {
        if (!Levels.ContainsKey(progress.Level))
        {
            Debug.Print("Error saving. Data for level " + progress.Level + " cannot be saved.");
            return false;
        }

        if (progress.Level > 1 && !Levels.ContainsKey(progress.Level - 1) && !Levels[progress.Level].Reached)
        {
            Debug.Print("Error saving. Level " + (progress.Level - 1) + " has not been completed yet.");
            return false;
        }

        var currentData = Levels[progress.Level];

        currentData.Ball1 = currentData.Ball1 || progress.Ball1;
        currentData.Ball2 = currentData.Ball2 || progress.Ball2;
        currentData.Ball3 = currentData.Ball3 || progress.Ball3;

        currentData.MaxScore = Math.Max(currentData.MaxScore, progress.Score);

        if (Levels.ContainsKey(progress.Level + 1))
        {
            Levels[progress.Level + 1].Reached = true;
        }
        return true;
    }

    public bool GetBallStatus(int level, int ballNr)
    {
        return ballNr switch
        {
            1 => Levels[level].Ball1,
            2 => Levels[level].Ball2,
            3 => Levels[level].Ball3,
            _ => false,
        };
    }

    public int GetGoldRest()
    {
        var allBallsCount = Levels.Sum(v =>
        {
            var count = 0;
            if (v.Value.Ball1) count++;
            if (v.Value.Ball2) count++;
            if (v.Value.Ball3) count++;
            return count;
        });

        var slotsCost = 0;
        for (int i = 0; i < Upgrades.Slots.Count; i++)
        {
            slotsCost += UpgradeItemInfo.SlotsCost[i];
        }

        var upgradesCost = 0;
        foreach (var upgrade in Upgrades.PurchasedUpgrades)
        {
            for (int i = 0; i < upgrade.CurrentLevel; i++)
            {
                upgradesCost += UpgradeItemInfo.UpgradeItemInfos[upgrade.Type].Cost[upgrade.CurrentLevel];
            }
        }

        return allBallsCount - slotsCost - upgradesCost;
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
    private CurrentGameData Game { get; set; }

    public CurrentGame()
    {
        Game = new(new CurrentGameDataBuilder());
    }

    public void CreateNewGame()
    {
        Game = new(new CurrentGameDataBuilder());
        GameComponets.Instance.Get<GameProgress>().Save(Game.ToGameProgressData());
    }

    public void ResetGame()
    {
        Game = new(new CurrentGameDataBuilder());
    }

    public void LoadNewGame(GameProgressData data)
    {
        var builder = new CurrentGameDataBuilder()
        {
            Id = data.Id
        };

        bool lastLvl = false;
        for (int i = 1; i < GameScenePaths.LEVELS.Count; i++)
        {
            if (!lastLvl && data.Levels.ContainsKey(i))
            {
                builder.Levels[i] = new LevelProgressData(data.Levels[i]);
            }
            else
            {
                lastLvl = true;
            }

            if (lastLvl)
            {
                builder.Levels[i] = new LevelProgressData(i);
            }
        }

        builder.Upgrades = data.Upgrades.Copy();

        Game = new(builder);
    }

    public void SaveProgress(CurrentProgress progress)
    {
        if (Game.UpdateGameData(progress))
        {
            GameComponets.Instance.Get<GameProgress>().Save(Game.ToGameProgressData());
        }
    }

    public bool GetBallStatus(int level, int ballNr) => Game.GetBallStatus(level, ballNr);

    public UpgradeData GetUpgradeData() => Game.Upgrades;

    public UpgradeController GetUpgradeController() => Game.UpgradeController;

    public int GetGoldRest() => Game.GetGoldRest();

    public IDictionary<int, LevelProgressData> GetLevels()
    {
        var response = new Dictionary<int, LevelProgressData>();
        foreach (var item in Game.Levels)
        {
            response.Add(item.Key, item.Value.Copy());
        }
        return response;
    }
}
