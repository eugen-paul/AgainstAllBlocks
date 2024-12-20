using System;
using Godot;

public partial class UpgradeItemPanel : Panel, IUpgradeListener
{
    private static readonly string UPGRADE_ICON_PATH = "TextureRect";
    private static readonly string UPGRADE_LABEL_PATH = "Label";
    private static readonly string UPGRADE_BUTTON_PATH = "UpgradeButton";
    private static readonly string INFO_BUTTON_PATH = "InfoButton";

    public GodotObject LocalizationScriptObject { get; set; } = null;

    public Action<UpgradeType> UpgradeAction { set; get; }

    public UpgradeType Type { get; private set; }

    public override void _EnterTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().AddListener(this);
        UpdateData();
    }

    public override void _ExitTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().RemoveListener(this);
    }

    public void Init(UpgradeType type)
    {
        Type = type;
    }

    private void UpdateData()
    {
        var level = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentUpgradeLevel(Type);
        var levelDescription = UpgradeItemInfo.UpgradeItemInfos[Type].LevelDescription[level];

        GetNode<UpgradeItemIcon>(UPGRADE_ICON_PATH).Init(Type);
        GetNode<Label>(UPGRADE_LABEL_PATH).Text = Var(levelDescription);

        var maxLevelReached = level >= UpgradeItemInfo.UpgradeItemInfos[Type].MaxLevel;
        GetNode<Button>(UPGRADE_BUTTON_PATH).Visible = !maxLevelReached;
        GetNode<Button>(INFO_BUTTON_PATH).Visible = maxLevelReached;
    }

    private string Var(string variableName)
    {
        if (LocalizationScriptObject == null)
        {
            return string.Empty;
        }
        return LocalizationScriptObject.Get(variableName).AsString();
    }

    private void OnUpgradeButtonPressed()
    {
        UpgradeAction?.Invoke(Type);
    }

    public void UpgrageDataChange(AUpgradeSignal upgradeSignal)
    {
        if (upgradeSignal is UpgradeSignalUpdateUpgradeData signal && signal.ItemType == Type)
        {
            UpdateData();
        }
    }

}
