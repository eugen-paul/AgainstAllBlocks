using System.Collections.Generic;
using Godot;

public partial class Levels : CanvasLayer
{
    private const string LEVELS_CONTAINER_PATH = "Main/Panel/ScrollContainer/VBoxContainer";

    private enum MenuOptions
    {
        EXIT_FRAME
    }

    private readonly Dictionary<MenuOptions, string> MenuOptionNames = new()
    {
        { MenuOptions.EXIT_FRAME, "ExitFrame" },
    };

    [Export]
    public PackedScene LevelSelectionScene { get; set; }

    public override void _Ready()
    {
        var gameData = GameComponets.Instance.Get<CurrentGame>().Game;

        var lvlContainer = GetNode<VBoxContainer>(LEVELS_CONTAINER_PATH);
        foreach (var child in lvlContainer.GetChildren())
        {
            child.QueueFree();
        }

        for (int i = 1; i < gameData.Levels.Count; i++)
        {
            var lvl = LevelSelectionScene.Instantiate<LevelProgress>();
            lvl.Init(gameData.Levels[i]);
            lvlContainer.AddChild(lvl);
        }

        HideAll();
    }

    private void ShowOnly(MenuOptions name)
    {
        foreach (var entry in MenuOptionNames)
        {
            if (entry.Key == name)
            {
                GetNode<Control>(entry.Value).Show();
            }
            else
            {
                GetNode<Control>(entry.Value).Hide();
            }
        }
    }

    private void HideAll()
    {
        foreach (var entry in MenuOptionNames)
        {
            GetNode<Control>(entry.Value).Hide();
        }
    }

    private void OnMenuButtonPressed()
    {
        ShowOnly(MenuOptions.EXIT_FRAME);
    }

    private void OnExitNoButtonPressed()
    {
        HideAll();
    }

    private void OnExitYesButtonPressed()
    {
        var next = ResourceLoader.Load<PackedScene>(GameScenePaths.MAIN_SCENE);
        GameComponets.Instance.Get<CurrentGame>().ResetGame();
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
    }
}
