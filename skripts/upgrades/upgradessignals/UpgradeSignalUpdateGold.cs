
public class UpgradeSignalUpdateGold : AUpgradeSignal
{
    public int Gold { private set; get; }
    public UpgradeSignalUpdateGold(int Gold) : base(UpgradeSignalType.UPDATE_GOLD)
    {
        this.Gold = Gold;
    }
}