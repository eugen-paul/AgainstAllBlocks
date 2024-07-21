using System.Diagnostics;
using System.Text.Json;
using Godot;

public class UserPreferencesData
{
    public bool ShowFps { get; set; } = false;
    public bool ShowBackground { get; set; } = true;
}

public enum UserPreferenceParams
{
    SHOW_FPS,
}

public class UserPreferences : GameComponet
{
    private UserPreferencesData data;

    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    private const string DIR_PATH = "user://";
    private const string FULL_PATH = DIR_PATH + "user_prefs.json";

    public UserPreferences()
    {
        CreateOrLoad();
    }

    public bool GetParamShowFps() => data.ShowFps;

    public void SetParamShowFps(bool value)
    {
        data.ShowFps = value;
        Save();
    }

    public bool GetParamShowBackground() => data.ShowBackground;

    public void SetParamShowBackground(bool value)
    {
        data.ShowBackground = value;
        Save();
    }

    private void Save()
    {
        string jsonString = JsonSerializer.Serialize(data, _options);
        FileAccess file = null;

        var dir = DirAccess.Open(DIR_PATH);
        var error = dir.MakeDirRecursive(DIR_PATH);
        if (error != Error.Ok)
        {
            Debug.Print("Can't create User folder: " + DIR_PATH);
            return;
        }

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
                data = JsonSerializer.Deserialize<UserPreferencesData>(jsonString);
            }
            catch
            {
                Debug.Print("Can't load UserPreferencesData File.");
                data = new();
                Save();
            }
            finally
            {
                file?.Close();
            }
        }
    }

}