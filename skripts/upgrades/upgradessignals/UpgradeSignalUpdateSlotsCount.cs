
public class UpgradeSignalUpdateSlotsCount : AUpgradeSignal
{
    public int SlotsCount { private set; get; }
    public UpgradeSignalUpdateSlotsCount(int SlotsCount) : base(UpgradeSignalType.UPDATE_SLOTS_COUNT)
    {
        this.SlotsCount = SlotsCount;
    }
}