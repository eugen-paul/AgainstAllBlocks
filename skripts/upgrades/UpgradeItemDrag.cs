
using Godot;

public partial class UpgradeItemDrag : GodotObject
{
    public static readonly string DEFAULT_UPGRADE_PREVIEW_PATH = "res://scenes/upgrades/UpgradeItemPreview.tscn";

    public UpgradeItemPreview Preview { get; private set; }

    public UpgradeType Type { get; private set; }

    [Export]
    public PackedScene UpgradeSlotScene { get; set; } = ResourceLoader.Load<PackedScene>(DEFAULT_UPGRADE_PREVIEW_PATH);

    public void Init(UpgradeType type)
    {
        Preview = UpgradeSlotScene.Instantiate<UpgradeItemPreview>();
        var level = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentUpgradeLevel(type);
        var texturePath = UpgradeItemInfo.UpgradeItemInfos[type].Textures[level];
        Preview.SetUpgrade(texturePath);
        Type = type;
    }
}