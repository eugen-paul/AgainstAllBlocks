
public class DeathBehavior : ItemBehavior
{
    private readonly AbstractLevel level;

    public DeathBehavior(AbstractLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.GetPaddle().PlayEvilLaugh();
        level.Death();
    }
}