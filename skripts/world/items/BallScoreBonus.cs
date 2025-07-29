
public class BallScoreBonus : ItemBehavior
{
    private readonly DefaultLevel level;

    public BallScoreBonus(DefaultLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        foreach (var ball in level.GetAllBalls())
        {
            ball.Type = BallType.SCORE_BONUS;
        }
    }
}