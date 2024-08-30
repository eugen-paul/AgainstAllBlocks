
public class PaddleSpeedIncrease : ItemBehavior
{
    private readonly AbstractLevel level;

    public PaddleSpeedIncrease(AbstractLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.GetPaddle().AddSpeed(+10);
    }
}