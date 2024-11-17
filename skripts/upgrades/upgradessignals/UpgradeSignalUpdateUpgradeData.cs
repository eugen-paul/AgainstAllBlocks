
public class UpgradeSignalUpdateUpgradeData : AUpgradeSignal
{
    public UpgradeType ItemType { private set; get; }

    public UpgradeSignalUpdateUpgradeData(UpgradeType ItemType) : base(UpgradeSignalType.UPDATE_UPGRADE_DATA)
    {
        this.ItemType = ItemType;
    }
}