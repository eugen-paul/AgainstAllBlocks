using System;
using System.Linq;
using Godot;

public partial class Upgrades : Control
{
    private readonly string UPGRADE_PANEL_PATH = "ScrollContainer/VBoxContainer";
    public Action CloseAction;

    [Export]
    public PackedScene UpgradeScene { get; set; }

    public override void _Ready()
    {
        GetNode<Node>(UPGRADE_PANEL_PATH).GetChildren().ToList().ForEach(c => c.QueueFree());

        var factory = new UpgradeFactory();
        var upgrades = factory.GetListOfUpgrades();
        foreach (var item in upgrades)
        {
            GD.Print("level " + item.CurrentLevel);
            var panel = UpgradeScene.Instantiate<UpgradeItemPanel>();
            panel.Item = item;
            GetNode<Node>(UPGRADE_PANEL_PATH).AddChild(panel);
        }
    }

    private void OnOkButtonPressed()
    {
        CloseAction.Invoke();
    }
}
