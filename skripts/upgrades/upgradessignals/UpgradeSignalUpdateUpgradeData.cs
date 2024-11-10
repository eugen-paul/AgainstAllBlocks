
public class UpgradeSignalUpdateUpgradeData : AUpgradeSignal
{
    public UpgradeType ItemType { private set; get; }
    public int Level { private set; get; }

    public UpgradeSignalUpdateUpgradeData(UpgradeType ItemType, int Level) : base(UpgradeSignalType.UPDATE_UPGRADE_DATA)
    {
        this.ItemType = ItemType;
        this.Level = Level;
    }
}