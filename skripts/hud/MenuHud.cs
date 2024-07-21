using System.Collections.Generic;
using Godot;


public partial class MenuHud : CanvasLayer
{
    private enum MenuOptions
    {
        MAIN, GAME, SETTING, HIGHSCORE
    }

    private readonly Dictionary<MenuOptions, string> MenuOptionNames = new()
    {
        { MenuOptions.MAIN, "Main" },
        { MenuOptions.GAME, "Game" },
        { MenuOptions.SETTING, "Settings" },
        { MenuOptions.HIGHSCORE, "Highscore" },
    };

    private const string PREFERENCE_SHOW_FPS = "Settings/CenterContainer/VBoxContainer/GridContainer/ShowFpsCheckBox";

    private UserPreferences userPreferences;

    public override void _Ready()
    {
        userPreferences = UserPreferences.LoadOrCreate();
        GetNode<CheckBox>(PREFERENCE_SHOW_FPS).SetPressedNoSignal(userPreferences.ShowFps);
        ShowOnly(MenuOptions.MAIN);
    }

    private void OnStartLoadGameButtonPressed()
    {
        var menu = ResourceLoader.Load<PackedScene>(GamePaths.getLevelPath(1));
        GetTree().ChangeSceneToPacked(menu);
    }

    public void OnSettingsButtonPressed()
    {
        ShowOnly(MenuOptions.SETTING);
    }

    public void OnSettingsBackPressed()
    {
        ShowOnly(MenuOptions.MAIN);
    }

    private void ShowOnly(MenuOptions name)
    {
        foreach (var entry in MenuOptionNames)
        {
            if (entry.Key == name)
            {
                GetNode<CanvasLayer>(entry.Value).Show();
            }
            else
            {
                GetNode<CanvasLayer>(entry.Value).Hide();
            }
        }
    }

    public void TogleShowFps(bool value)
    {
        userPreferences.ShowFps = value;
        userPreferences.Save();
    }
}
