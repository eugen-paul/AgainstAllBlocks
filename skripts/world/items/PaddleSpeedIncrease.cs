
public class PaddleSpeedIncrease : ItemBehavior
{
    private readonly DefaultLevel level;

    public PaddleSpeedIncrease(DefaultLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.GetPaddle().AddSpeed(+10);
    }
}