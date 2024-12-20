using System.Collections.Generic;
using Godot;

public partial class Levels : CanvasLayer
{
    private const string LEVELS_CONTAINER_PATH = "Main/Panel/ScrollContainer/VBoxContainer";
    private const string UPGRADES_PATH = "UpgradesLayer/Upgrades";

    private enum MenuOptions
    {
        UPGRADE_FRAME,
        EXIT_FRAME
    }

    private readonly Dictionary<MenuOptions, string> MenuOptionNames = new()
    {
        { MenuOptions.UPGRADE_FRAME, "UpgradesLayer" },
        { MenuOptions.EXIT_FRAME, "ExitFrameLayer" },
    };

    [Export]
    public PackedScene LevelSelectionScene { get; set; }

    public override void _Ready()
    {
        var levelsData = GameComponets.Instance.Get<CurrentGame>().GetLevels();

        var lvlContainer = GetNode<VBoxContainer>(LEVELS_CONTAINER_PATH);
        foreach (var child in lvlContainer.GetChildren())
        {
            child.QueueFree();
        }

        for (int i = 1; i < levelsData.Count; i++)
        {
            var lvl = LevelSelectionScene.Instantiate<LevelProgress>();
            lvl.Init(levelsData[i]);
            lvlContainer.AddChild(lvl);
        }

        GetNode<Upgrades>(UPGRADES_PATH).CloseUpgradeMenuAction += HideAll;

        HideAll();
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

    private void HideAll()
    {
        foreach (var entry in MenuOptionNames)
        {
            GetNode<CanvasLayer>(entry.Value).Hide();
        }
    }

    private void OnMenuButtonPressed()
    {
        ShowOnly(MenuOptions.EXIT_FRAME);
    }

    private void OnUpgradesButtonPressed()
    {
        ShowOnly(MenuOptions.UPGRADE_FRAME);
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
