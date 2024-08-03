using System;
using System.Collections.Generic;

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
    }

    public void SaveAndCloseGame()
    {
        SaveProgress();
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
    }

    public void SaveProgress()
    {

    }
}

public class CurrentGameData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Dictionary<int, LevelProgressData> Levels { get; set; } = new();

    public CurrentGameData()
    {
        for (int i = 1; i < GameScenePaths.LEVELS.Count; i++)
        {
            Levels[i] = new LevelProgressData(i, i == 1);
        }
    }
}