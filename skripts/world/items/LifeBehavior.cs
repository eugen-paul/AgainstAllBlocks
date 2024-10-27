
public class LifeBehavior : ItemBehavior
{
    private readonly int value;

    private readonly DefaultLevel level;

    public LifeBehavior(int value, DefaultLevel level)
    {
        this.value = value;
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.LifesAdd(value);
    }
}