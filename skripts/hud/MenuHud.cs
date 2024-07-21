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
    private const string PREFERENCE_SHOW_BG = "Settings/CenterContainer/VBoxContainer/GridContainer/ShowBgCheckBox";

    public override void _Ready()
    {
        var userPreferences = GameComponets.Instance.Get<UserPreferences>();
        GetNode<CheckBox>(PREFERENCE_SHOW_FPS).SetPressedNoSignal(userPreferences.GetParamShowFps());
        if (userPreferences.GetParamShowFps())
        {
            GetNode<CanvasLayer>("Fps").Show();
        }

        GetNode<CheckBox>(PREFERENCE_SHOW_BG).SetPressedNoSignal(userPreferences.GetParamShowBackground());
        ShowOnly(MenuOptions.MAIN);
    }

    private void OnStartLoadGameButtonPressed()
    {
        var menu = ResourceLoader.Load<PackedScene>(GameScenePaths.getLevelPath(1));
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
        var userPreferences = GameComponets.Instance.Get<UserPreferences>();
        userPreferences.SetParamShowFps(value);
        if (value)
        {
            GetNode<CanvasLayer>("Fps").Show();
        }
        else
        {
            GetNode<CanvasLayer>("Fps").Hide();
        }
    }

    public static void TogleShowBg(bool value)
    {
        var userPreferences = GameComponets.Instance.Get<UserPreferences>();
        userPreferences.SetParamShowBackground(value);
    }

}
