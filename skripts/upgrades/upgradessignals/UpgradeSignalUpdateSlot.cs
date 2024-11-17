
public class UpgradeSignalUpdateSlot : AUpgradeSignal
{
    public int SlotNr { private set; get; }

    public UpgradeSignalUpdateSlot(int SlotNr) : base(UpgradeSignalType.UPDATE_SLOT)
    {
        this.SlotNr = SlotNr;
    }
}