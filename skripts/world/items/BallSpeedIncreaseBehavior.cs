
public class BallSpeedIncrease : ItemBehavior
{
    private readonly AbstractLevel level;

    public BallSpeedIncrease(AbstractLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        foreach (var ball in level.GetAllBalls())
        {
            ball.Velocity *= 1.2f;
        }
    }
}