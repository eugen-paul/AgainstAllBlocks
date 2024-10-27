
public class PaddleSpeedDecrease : ItemBehavior
{
    private readonly DefaultLevel level;

    public PaddleSpeedDecrease(DefaultLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.GetPaddle().AddSpeed(-10);
    }
}