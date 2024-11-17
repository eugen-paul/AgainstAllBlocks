using System.Linq;
using Godot;

public partial class UpgradeSlotsPanel : CenterContainer, IUpgradeListener
{
    private static readonly string DEFAULT_UPGRADE_SLOT_PANEL = "res://scenes/upgrades/UpgradeSlot.tscn";
    private static readonly string SLOTS_CONTAINER_PATH = "SlotsContainer";

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

    public override void _EnterTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().AddListener(this);
    }

    public override void _ExitTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().RemoveListener(this);
    }

    private void AddSlot(int slotNr, UpgradeType itemType)
    {
        var slot = UpgradeSlotScene.Instantiate<UpgradeSlot>();
        slot.Init(slotNr, itemType);
        GetNode<Node>(SLOTS_CONTAINER_PATH).AddChild(slot);
    }

    public void UpgrageDataChange(AUpgradeSignal upgradeSignal)
    {
        if (upgradeSignal is UpgradeSignalUpdateSlotsCount)
        {
            var upgradeController = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController();
            while (GetNode<Node>(SLOTS_CONTAINER_PATH).GetChildCount() < upgradeController.GetCurrentSlots().Count)
            {
                AddSlot(GetNode<Node>(SLOTS_CONTAINER_PATH).GetChildCount(), UpgradeType.EMPTY);
            }
        }
    }

}
