using System;
using System.Linq;
using Godot;

public partial class Upgrades : Control, IUpgradeListener
{
    private GodotObject LocalizationScriptObject;

    private static readonly string DEFAULT_UPGRADE_ITEM_PANEL = "res://scenes/upgrades/UpgradeItemPanel.tscn";
    private static readonly string DEFAULT_UPGRADE_ITEM_INFO_PANEL = "res://scenes/upgrades/UpgradeItemLevelPanel.tscn";
    private static readonly string PLUS_SLOT_BUTTON_PATH = "UpgradeMenu/CenterContainer/HBoxContainer/PlusSlotButton";
    private static readonly string UPGRADE_PANEL_PATH = "UpgradeMenu/ScrollContainer/VBoxContainer";
    private static readonly string UPGRADE_ITEM_MENU_DESCRIPTION_PATH = "UpgradeItemMenu/Panel/Panel/VBoxContainer/PanelContainer/Label";
    private static readonly string UPGRADE_ITEM_MENU_CONTAINER_PATH = "UpgradeItemMenu/Panel/Panel/VBoxContainer/ScrollContainer/VBoxContainer";
    private static readonly string BUY_SLOT_MENU_PATH = "BuySlotMenu";
    private static readonly string BUY_SLOT_TEXT_PATH = "BuySlotMenu/Panel/SlotBuyLabel";
    private static readonly string BUY_SLOT_BUTTON_PATH = "BuySlotMenu/Panel/BuySlotButton";
    private static readonly string BUY_SLOTMAX_TEXT_PATH = "BuySlotMenu/Panel/SlotMaxLabel";
    private static readonly string BUY_SLOT_TEXT = "UI_SLOTBUY";
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

        var upgradeController = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController();
        var upgrades = upgradeController.GetUpgradesOrder();
        foreach (var upgradeType in upgrades)
        {
            var panel = UpgradeScene.Instantiate<UpgradeItemPanel>();
            panel.Init(upgradeType);
            panel.UpgradeAction += ShowUpgradeItemMenu;
            panel.LocalizationScriptObject = LocalizationScriptObject;
            GetNode<Node>(UPGRADE_PANEL_PATH).AddChild(panel);
        }

        SetUpgradeItemMenuVisibile(false);
        SetBuySlotMenuVisibile(false);
        CheckAndSetUpgradeButtonVisible();
    }

    public override void _EnterTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().AddListener(this);
    }

    public override void _ExitTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().RemoveListener(this);
    }

    private void SetUpgradeItemMenuVisibile(bool visible)
    {
        GetNode<Control>(UPGRADE_ITEM_MENU_PATH).Visible = visible;
    }

    private void SetBuySlotMenuVisibile(bool visible)
    {
        GetNode<Control>(BUY_SLOT_MENU_PATH).Visible = visible;

        GetNode<Control>(BUY_SLOT_TEXT_PATH).Visible = !MaxSlotsReached();
        GetNode<Control>(BUY_SLOTMAX_TEXT_PATH).Visible = MaxSlotsReached();
    }

    private static bool MaxSlotsReached()
    {
        var upgradeController = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController();
        return upgradeController.GetCurrentSlots().Count >= UpgradeItemInfo.SlotsCost.Count;
    }

    private void OnOkButtonPressed()
    {
        CloseAction.Invoke();
    }

    private void CheckAndSetUpgradeButtonVisible()
    {
        GetNode<Control>(PLUS_SLOT_BUTTON_PATH).Visible = !MaxSlotsReached();
    }

    private void ShowUpgradeItemMenu(UpgradeType upgradeType)
    {
        var upgradeController = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController();
        var description = UpgradeItemInfo.UpgradeItemInfos[upgradeType].Description;
        var maxLevel = UpgradeItemInfo.UpgradeItemInfos[upgradeType].MaxLevel;

        GetNode<Label>(UPGRADE_ITEM_MENU_DESCRIPTION_PATH).Text = LocalizationScriptObject.Get(description).AsString();

        GetNode<Node>(UPGRADE_ITEM_MENU_CONTAINER_PATH).GetChildren().ToList().ForEach(c => c.QueueFree());

        for (int level = 0; level < maxLevel; level++)
        {
            var upgradePanel = ItemInfoScene.Instantiate<UpgradeItemLevelPanel>();
            upgradePanel.Name = "Level" + level;
            upgradePanel.Init(upgradeType, level);
            upgradePanel.LocalizationScriptObject = LocalizationScriptObject;
            GetNode<Node>(UPGRADE_ITEM_MENU_CONTAINER_PATH).AddChild(upgradePanel);
        }
        SetUpgradeItemMenuVisibile(true);
    }

    private void OnCloseUpgradeItemMenuButtonPressed()
    {
        SetUpgradeItemMenuVisibile(false);
    }

    public void UpgrageDataChange(AUpgradeSignal upgradeSignal)
    {
        if (upgradeSignal is UpgradeSignalUpdateSlotsCount)
        {
            CheckAndSetUpgradeButtonVisible();
            UpdateSlotBuyMenu();
        }
    }

    private void OnPlusSlotButtonPressed()
    {
        SetBuySlotMenuVisibile(true);
        UpdateSlotBuyMenu();
    }

    private void OnCloseBuySlotMenuButtonPressed()
    {
        SetBuySlotMenuVisibile(false);
    }

    private void OnBuySlotButtonPressed()
    {
        var upgradeController = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController();
        upgradeController.BuyNewUpgradeSlot();
    }

    private void UpdateSlotBuyMenu()
    {
        if (!MaxSlotsReached())
        {
            var buySlotText = LocalizationScriptObject.Get(BUY_SLOT_TEXT).AsString();
            var upgradeController = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController();
            var nextSlotNr = upgradeController.GetCurrentSlots().Count;
            var nextSlotCost = UpgradeItemInfo.SlotsCost[nextSlotNr];
            GetNode<Label>(BUY_SLOT_TEXT_PATH).Text = string.Format(buySlotText, nextSlotCost);
            GetNode<Button>(BUY_SLOT_BUTTON_PATH).Visible = true;

            var goldRest = GameComponets.Instance.Get<CurrentGame>().GetGoldRest();
            GetNode<Button>(BUY_SLOT_BUTTON_PATH).Disabled = goldRest < nextSlotCost;
        }
        else
        {
            GetNode<Button>(BUY_SLOT_BUTTON_PATH).Visible = false;
        }
    }
}
