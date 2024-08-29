
public class LifeBehavior : ItemBehavior
{
    private readonly int value;

    private readonly AbstractLevel level;

    public LifeBehavior(int value, AbstractLevel level)
    {
        this.value = value;
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.LifesAdd(value);
    }
}