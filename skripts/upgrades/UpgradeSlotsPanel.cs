using System.Linq;
using Godot;

public partial class UpgradeSlotsPanel : CenterContainer
{
    private static readonly string DEFAULT_UPGRADE_SLOT_PANEL = "res://scenes/upgrades/UpgradeSlot.tscn";
    private static readonly string SLOTS_CONTAINER_PATH = "HBoxContainer/SlotsContainer";
    private static readonly string PLUS_SLOT_BUTTON_PATH = "HBoxContainer/PlusSlotButton";

    [Export]
    public PackedScene UpgradeSlotScene { get; set; } = ResourceLoader.Load<PackedScene>(DEFAULT_UPGRADE_SLOT_PANEL);

    public override void _Ready()
    {
        GetNode<Node>(SLOTS_CONTAINER_PATH).GetChildren().ToList().ForEach(c => c.QueueFree());

        var upgradeController = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController();
        for (int slotNr = 0; slotNr < upgradeController.GetCurrentSlots().Count; slotNr++)
        {
            AddSlot(slotNr, upgradeController.GetCurrentSlots()[slotNr]);
        }
    }

    public void AddSlot(int slotNr, UpgradeType itemType)
    {
        var slot = UpgradeSlotScene.Instantiate<UpgradeSlot>();
        slot.Init(slotNr, itemType);
        GetNode<Node>(SLOTS_CONTAINER_PATH).AddChild(slot);
    }

    public void SetUpgradeButtonVisible(bool visible)
    {
        GetNode<Control>(PLUS_SLOT_BUTTON_PATH).Visible = visible;
    }
}
