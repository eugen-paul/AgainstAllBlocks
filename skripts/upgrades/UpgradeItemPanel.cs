using System;
using Godot;

public partial class UpgradeItemPanel : PanelContainer, IUpgradeListener
{
    private static readonly string UPGRADE_PANEL_PATH = "Panel/HBoxContainer/Panel";
    private static readonly string UPGRADE_ICON_PATH = "Panel/HBoxContainer/Panel/TextureRect";
    private static readonly string UPGRADE_LABEL_PATH = "Panel/HBoxContainer/Label";
    private static readonly string UPGRADE_BUTTON_PATH = "Panel/HBoxContainer/UpgradeButton";
    private static readonly string INFO_BUTTON_PATH = "Panel/HBoxContainer/InfoButton";

    public GodotObject LocalizationScriptObject { get; set; } = null;

    public Action<UpgradeType> UpgradeAction { set; get; }

    public UpgradeType Type { get; private set; }

    public override void _EnterTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().AddListener(this);
        UpdateData();

        var slotCount = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentSlots().Count;
        if (slotCount > 0)
        {
            GetNode<Button>(UPGRADE_BUTTON_PATH).Disabled = false;
            GetNode<Button>(INFO_BUTTON_PATH).Disabled = false;
        }
        else
        {
            GetNode<Button>(UPGRADE_BUTTON_PATH).Disabled = true;
            GetNode<Button>(INFO_BUTTON_PATH).Disabled = true;
        }

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

        if (Type != UpgradeType.EMPTY && level == 0)
        {
            GetNode<Panel>(UPGRADE_PANEL_PATH).Modulate = new Color(1, 1, 1, 0.5f);
        } else {
            GetNode<Panel>(UPGRADE_PANEL_PATH).Modulate = new Color(1, 1, 1, 1f);
        }

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
        else if (upgradeSignal is UpgradeSignalUpdateSlotsCount)
        {
            var slotCount = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentSlots().Count;
            if (slotCount > 0)
            {
                GetNode<Button>(UPGRADE_BUTTON_PATH).Disabled = false;
                GetNode<Button>(INFO_BUTTON_PATH).Disabled = false;
            }
            else
            {
                GetNode<Button>(UPGRADE_BUTTON_PATH).Disabled = true;
                GetNode<Button>(INFO_BUTTON_PATH).Disabled = true;
            }
        }
    }

}
