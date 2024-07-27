using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Godot;

public class GameProgressData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int TotalScore { get; set; } = 0;
    public int Level { get; set; } = 1;
}

public class GameProgressDataGlobal
{
    public List<GameProgressData> games { get; set; } = new();
}

public class GameProgress : GameComponet
{
    private GameProgressDataGlobal data;

    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    private readonly string FULL_PATH = PreferencePaths.CONFIG_DIR_PATH + "gameProgress.json";

    public GameProgress()
    {
        CreateOrLoad();
    }

    private void Save()
    {
        string jsonString = JsonSerializer.Serialize(data, _options);
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
        return data.games;
    }

}