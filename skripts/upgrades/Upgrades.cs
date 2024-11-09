using System;
using System.Linq;
using Godot;

public partial class Upgrades : Control
{
    private GodotObject LocalizationScriptObject;

    private static readonly string DEFAULT_UPGRADE_ITEM_PANEL = "res://scenes/upgrades/UpgradeItemPanel.tscn";
    private static readonly string DEFAULT_UPGRADE_ITEM_INFO_PANEL = "res://scenes/upgrades/UpgradeItemLevelPanel.tscn";
    private static readonly string UPGRADE_PANEL_PATH = "UpgradeMenu/ScrollContainer/VBoxContainer";
    private static readonly string UPGRADE_ITEM_MENU_DESCRIPTION_PATH = "UpgradeItemMenu/Panel/Panel/VBoxContainer/PanelContainer/Label";
    private static readonly string UPGRADE_ITEM_MENU_CONTAINER_PATH = "UpgradeItemMenu/Panel/Panel/VBoxContainer/ScrollContainer/VBoxContainer";
    private static readonly string UPGRADE_ITEM_MENU_PATH = "UpgradeItemMenu";
    public Action CloseAction;

    [Export]
    public PackedScene UpgradeScene { get; set; } = ResourceLoader.Load<PackedScene>(DEFAULT_UPGRADE_ITEM_PANEL);

    [Export]
    public PackedScene ItemInfoScene { get; set; } = ResourceLoader.Load<PackedScene>(DEFAULT_UPGRADE_ITEM_INFO_PANEL);

    public override void _Ready()
    {
        GDScript script = GD.Load<GDScript>("res://scenes/localization/Localization_Upgrades.gd");
        LocalizationScriptObject = (GodotObject)script.New();

        GetNode<Node>(UPGRADE_PANEL_PATH).GetChildren().ToList().ForEach(c => c.QueueFree());

        var factory = new UpgradeFactory();
        var upgrades = factory.GetListOfUpgrades();
        foreach (var item in upgrades)
        {
            var panel = UpgradeScene.Instantiate<UpgradeItemPanel>();
            panel.Item = item;
            panel.UpgradeAction += ShowUpgradeItemMenu;
            panel.LocalizationScriptObject = LocalizationScriptObject;
            GetNode<Node>(UPGRADE_PANEL_PATH).AddChild(panel);
        }

        ShowUpgradeItemMenu(false);
    }

    private void ShowUpgradeItemMenu(bool visible)
    {
        GetNode<Control>(UPGRADE_ITEM_MENU_PATH).Visible = visible;
    }

    private void OnOkButtonPressed()
    {
        CloseAction.Invoke();
    }

    private void ShowUpgradeItemMenu(AUpgrade item)
    {
        GetNode<Label>(UPGRADE_ITEM_MENU_DESCRIPTION_PATH).Text = LocalizationScriptObject.Get(item.Description).AsString();

        GetNode<Node>(UPGRADE_ITEM_MENU_CONTAINER_PATH).GetChildren().ToList().ForEach(c => c.QueueFree());

        for (int level = 0; level <= item.MaxLevel; level++)
        {
            var upgradePanel = ItemInfoScene.Instantiate<UpgradeItemLevelPanel>();
            upgradePanel.Name = "Level" + level;
            upgradePanel.Level = level;
            upgradePanel.TexturePath = item.Textures[level];
            upgradePanel.LevelDescription = item.LevelDescription[level];
            upgradePanel.Purchased = item.CurrentLevel >= level;
            upgradePanel.LocalizationScriptObject = LocalizationScriptObject;

            GetNode<Node>(UPGRADE_ITEM_MENU_CONTAINER_PATH).AddChild(upgradePanel);
        }
        ShowUpgradeItemMenu(true);
    }

    private void OnCloseUpgradeItemMenuButtonPressed()
    {
        ShowUpgradeItemMenu(false);
    }
}
