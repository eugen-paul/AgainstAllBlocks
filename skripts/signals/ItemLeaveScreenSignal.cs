public class ItemLeaveScreenSignal : AbstractSignal
{
    public ItemType Type { private set; get; }

    public ItemLeaveScreenSignal(ItemType itemType)
    {
        Type = itemType;
    }
}