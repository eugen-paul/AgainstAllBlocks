
public class UpgradeSignalUpdateSlot : AUpgradeSignal
{
    public int SlotNr { private set; get; }
    public UpgradeType ItemType { private set; get; }
    public int Level { private set; get; }

    public UpgradeSignalUpdateSlot(int SlotNr, UpgradeType ItemType, int Level) : base(UpgradeSignalType.UPDATE_SLOT)
    {
        this.SlotNr = SlotNr;
        this.ItemType = ItemType;
        this.Level = Level;
    }
}