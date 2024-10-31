using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Godot;

public class GameProgressData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Dictionary<int, LevelProgressData> Levels { get; set; } = new();

    public UpgradeData Upgrades { get; set; } = new();

    public GameProgressData() { }
}

public class LevelProgressData
{
    public int Level { get; set; } = 1;
    public bool Reached { get; set; } = false;
    public int MaxScore { get; set; } = 0;
    public bool Ball1 { get; set; } = false;
    public bool Ball2 { get; set; } = false;
    public bool Ball3 { get; set; } = false;

    public LevelProgressData() { }

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

    public LevelProgressData Copy()
    {
        return new LevelProgressData(this);
    }
}

public class GameProgressDataGlobal
{
    public List<GameProgressData> Games { get; set; } = new();
}

public class GameProgress : GameComponet
{
    public static readonly int MAX_GAMES = 3;

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
            Debug.Print("Save game not found. Create a new save game file.");
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
                Debug.Print("loaded " + data.Games.Count + " saves");
            }
            catch (Exception ex)
            {
                Debug.Print("Can't load GameProgress File.");
                Debug.WriteLine(ex.ToString());
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

    public void Save(GameProgressData dataToSave)
    {
        var game = data.Games.Find(x => x.Id == dataToSave.Id);

        if (game == null)
        {
            if (data.Games.Count >= MAX_GAMES)
            {
                Debug.Print("Maximum number of save games reached.");
                return;
            }
        }
        else
        {
            data.Games.Remove(game);
        }
        data.Games.Add(dataToSave);
        Save();
    }

    public void Delete(Guid Id)
    {
        var game = data.Games.Find(x => x.Id == Id);

        if (game == null)
        {
            Debug.Print($"Cann't delte gaslot. Game with ID {Id} not found.");
            return;
        }
        else
        {
            Debug.Print($"Removing gameslot with ID {Id}.");
            data.Games.Remove(game);
        }
        Save();
    }

}