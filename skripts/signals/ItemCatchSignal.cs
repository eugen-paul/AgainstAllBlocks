public class ItemCatchSignal : AbstractSignal
{
    public ItemType Type { private set; get; }

    public ItemCatchSignal(ItemType itemType)
    {
        Type = itemType;
    }
}