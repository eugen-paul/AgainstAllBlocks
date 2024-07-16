using Godot;

public partial class UserPreferences : Resource
{
    [Export]
    public bool ShowFps { get; set; } = false;

    public void Save()
    {
        ResourceSaver.Save(this, "user://user_prefs.tres");
    }

    public static UserPreferences LoadOrCreate()
    {
        return GD.Load<UserPreferences>("user://user_prefs.tres") ?? new();
    }
}