
public class BallAddBehavior : ItemBehavior
{
    private readonly Item item;
    private readonly DefaultLevel level;

    public BallAddBehavior(Item item, DefaultLevel level)
    {
        this.item = item;
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.AddBall(item.GlobalPosition.X);
    }
}