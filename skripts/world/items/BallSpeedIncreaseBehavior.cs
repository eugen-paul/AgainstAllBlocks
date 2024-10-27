
public class BallSpeedIncrease : ItemBehavior
{
    private readonly DefaultLevel level;

    public BallSpeedIncrease(DefaultLevel level)
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