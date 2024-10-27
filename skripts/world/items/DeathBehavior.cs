
public class DeathBehavior : ItemBehavior
{
    private readonly DefaultLevel level;

    public DeathBehavior(DefaultLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.GetPaddle().PlayEvilLaugh();
        level.Death();
    }
}