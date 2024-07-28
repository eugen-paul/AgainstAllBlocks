using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Godot;

public class GameProgressData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int TotalScore { get; set; } = 0;
    public Dictionary<int, LevelProgressData> Levels { get; set; } = new();
}

public class LevelProgressData
{
    public int Level { get; set; } = 1;
    public bool Reached { get; set; } = false;
    public int MaxScore { get; set; } = 0;
    public bool Ball1 { get; set; } = false;
    public bool Ball2 { get; set; } = false;
    public bool Ball3 { get; set; } = false;

    public LevelProgressData(int level, bool reached = false)
    {
        Level = level;
        Reached = reached;
    }

    public LevelProgressData(LevelProgressData other)
    {
        Level = other.Level;
        Reached = other.Reached;
        MaxScore = other.MaxScore;
        Ball1 = other.Ball1;
        Ball2 = other.Ball2;
        Ball3 = other.Ball3;
    }
}

public class GameProgressDataGlobal
{
    public List<GameProgressData> Games { get; set; } = new();
}

public class GameProgress : GameComponet
{
    private GameProgressDataGlobal data;

    private readonly JsonSerializerOptions options = new() { WriteIndented = true };

    private readonly string FULL_PATH = PreferencePaths.CONFIG_DIR_PATH + "gameProgress.json";

    public GameProgress()
    {
        CreateOrLoad();
    }

    private void Save()
    {
        string jsonString = JsonSerializer.Serialize(data, options);
        FileAccess file = null;

        try
        {
            file = FileAccess.Open(FULL_PATH, FileAccess.ModeFlags.Write);
            file.StoreString(jsonString);
        }
        catch
        {
            Debug.Print("Can't save UserPreferencesData File.");
        }
        finally
        {
            file?.Close();
        }
    }

    private void CreateOrLoad()
    {
        if (!FileAccess.FileExists(FULL_PATH))
        {
            data = new();
            Save();
        }
        else
        {
            FileAccess file = null;
            try
            {
                file = FileAccess.Open(FULL_PATH, FileAccess.ModeFlags.Read);
                var jsonString = file.GetAsText();
                data = JsonSerializer.Deserialize<GameProgressDataGlobal>(jsonString);
            }
            catch
            {
                Debug.Print("Can't load GameProgress File.");
                data = new();
                Save();
            }
            finally
            {
                file?.Close();
            }
        }
    }

    public List<GameProgressData> GetGameProgresses()
    {
        return data.Games;
    }

}