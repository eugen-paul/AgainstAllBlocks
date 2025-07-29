
public class PaddleSpeedStop : ItemBehavior
{
    private readonly DefaultLevel level;

    public PaddleSpeedStop(DefaultLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.GetPaddle().RemoveSpeed();
    }
}