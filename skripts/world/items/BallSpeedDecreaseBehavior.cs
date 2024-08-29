
public class BallSpeedDecrease : ItemBehavior
{
    private readonly AbstractLevel level;

    public BallSpeedDecrease(AbstractLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        foreach (var ball in level.GetAllBalls())
        {
            ball.Velocity /= 1.2f;
        }
    }
}