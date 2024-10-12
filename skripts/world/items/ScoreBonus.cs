
public class ScoreBonus : ItemBehavior
{
    private readonly AbstractLevel level;

    public ScoreBonus(AbstractLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.GetPaddle().PlayCoinPickup();
        level.Score += 25;
    }
}