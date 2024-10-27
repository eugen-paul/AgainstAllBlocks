
public class BallSpeedDecrease : ItemBehavior
{
    private readonly DefaultLevel level;

    public BallSpeedDecrease(DefaultLevel level)
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