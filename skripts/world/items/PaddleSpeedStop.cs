
public class PaddleSpeedStop : ItemBehavior
{
    private readonly AbstractLevel level;

    public PaddleSpeedStop(AbstractLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.GetPaddle().AddSpeed(-10);
    }
}