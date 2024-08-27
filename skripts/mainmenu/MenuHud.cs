using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;


public partial class MenuHud : CanvasLayer
{

    [Export]
    public PackedScene GameSlotEmpty { get; set; }
    [Export]
    public PackedScene GameSlotSave { get; set; }

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
    private const string PREFERENCE_SHOW_SHADOW = "Settings/CenterContainer/VBoxContainer/GridContainer/ShowShadowCheckBox";
    private const string GAMEPROGRESS_GRID = "Game/CenterContainer/VBoxContainer/GameProgressGridContainer";

    public override void _Ready()
    {
        var userPreferences = GameComponets.Instance.Get<UserPreferences>();
        GetNode<CheckBox>(PREFERENCE_SHOW_FPS).SetPressedNoSignal(userPreferences.GetParamShowFps());
        if (userPreferences.GetParamShowFps())
        {
            GetNode<CanvasLayer>("Fps").Show();
        }

        GetNode<CheckBox>(PREFERENCE_SHOW_BG).SetPressedNoSignal(userPreferences.GetParamShowBackground());
        GetNode<CheckBox>(PREFERENCE_SHOW_SHADOW).SetPressedNoSignal(userPreferences.GetParamShowShadow());
        ShowOnly(MenuOptions.MAIN);
    }

    private void OnStartLoadGameButtonPressed()
    {
        LoadGameProgress();
        ShowOnly(MenuOptions.GAME);
    }

    private void LoadGameProgress()
    {
        var gameProgressGrid = GetNode<GridContainer>(GAMEPROGRESS_GRID);

        foreach (var child in gameProgressGrid.GetChildren())
        {
            child.QueueFree();
        }

        var gp = GameComponets.Instance.Get<GameProgress>();
        var games = gp.GetGameProgresses();
        foreach (var slot in games)
        {
            var gameSlot = GameSlotSave.Instantiate<GameSlotSave>();
            gameSlot.Init(slot, () => LoadGame(slot.Id), () => DeleteGame(slot.Id));
            gameProgressGrid.AddChild(gameSlot);
        }

        if (games.Count == 0 || games.Count < GameProgress.MAX_GAMES)
        {
            var gameSlot = GameSlotEmpty.Instantiate<GameSlotEmpty>();
            gameSlot.Init(() => NewGame());
            gameProgressGrid.CallDeferred("add_child", gameSlot);
        }
    }

    private void LoadGame(Guid id)
    {
        var games = GameComponets.Instance.Get<GameProgress>().GetGameProgresses();
        GameProgressData gameData = games.Find(g => g.Id == id);

        if (gameData == null)
        {
            Debug.Print("Cann't find Savegame with id " + id);
            return;
        }

        var next = ResourceLoader.Load<PackedScene>(GameScenePaths.LEVEL_SELECTION_SCENE);
        GameComponets.Instance.Get<CurrentGame>().LoadNewGame(gameData);
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
    }

    private void DeleteGame(Guid id)
    {
        var gp = GameComponets.Instance.Get<GameProgress>();
        gp.Delete(id);
        LoadGameProgress();
    }

    private void NewGame()
    {
        var next = ResourceLoader.Load<PackedScene>(GameScenePaths.LEVEL_SELECTION_SCENE);
        GameComponets.Instance.Get<CurrentGame>().CreateNewGame();
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
    }

    public void OnSettingsButtonPressed()
    {
        ShowOnly(MenuOptions.SETTING);
    }

    public void OnQuitButtonPressed()
    {
        // GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
        GetTree().Quit();
    }

    public void OnSettingsBackPressed()
    {
        ShowOnly(MenuOptions.MAIN);
    }

    public void OnGameBackPressed()
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

    public static void TogleShadow(bool value)
    {
        var userPreferences = GameComponets.Instance.Get<UserPreferences>();
        userPreferences.SetParamShowShadow(value);
    }

}
