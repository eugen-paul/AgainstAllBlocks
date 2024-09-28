using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

public class UserPreferencesData
{
    public bool ShowFps { get; set; } = false;
    public bool ShowBackground { get; set; } = true;
    public bool ShowShadow { get; set; } = true;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EffectsPreferences Effects { get; set; } = EffectsPreferences.HIGH;
}

public enum EffectsPreferences
{
    OFF = 0,
    LOW = 1,
    HIGH = 2,
}

public class UserPreferences : GameComponet
{
    private UserPreferencesData data;

    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    private readonly string FULL_PATH = PreferencePaths.CONFIG_DIR_PATH + "user_prefs.json";

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

    public bool GetParamShowShadow() => data.ShowShadow;

    public void SetParamShowShadow(bool value)
    {
        data.ShowShadow = value;
        Save();
    }

    public EffectsPreferences GetParamEffects() => data.Effects;

    public void SetParamEffects(EffectsPreferences value)
    {
        data.Effects = value;
        Save();
    }

    public void SetParamEffects(int value)
    {
        SetParamEffects((EffectsPreferences)value);
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