public enum UpgradeSignalType
{
    UPDATE_SLOTS_COUNT,
    UPDATE_SLOT,
    UPDATE_UPGRADE_DATA,
    UPDATE_GOLD,
}

public abstract class AUpgradeSignal
{
    public UpgradeSignalType Type { private set; get; }

    protected AUpgradeSignal(UpgradeSignalType Type)
    {
        this.Type = Type;
    }
}