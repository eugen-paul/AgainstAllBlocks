
public class ScoreBonus : ItemBehavior
{
    private readonly DefaultLevel level;

    public ScoreBonus(DefaultLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.GetPaddle().PlayCoinPickup();
        level.Score += 25;
    }
}