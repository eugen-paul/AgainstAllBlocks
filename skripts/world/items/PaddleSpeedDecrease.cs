
public class PaddleSpeedDecrease : ItemBehavior
{
    private readonly AbstractLevel level;

    public PaddleSpeedDecrease(AbstractLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.GetPaddle().AddSpeed(-10);
    }
}