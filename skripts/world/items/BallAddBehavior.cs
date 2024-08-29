
public class BallAddBehavior : ItemBehavior
{
    private readonly Item item;
    private readonly AbstractLevel level;

    public BallAddBehavior(Item item, AbstractLevel level)
    {
        this.item = item;
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.AddBall(item.GlobalPosition.X);
    }
}