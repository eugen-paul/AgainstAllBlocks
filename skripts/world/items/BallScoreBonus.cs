
using Godot;

public class BallScoreBonus : ItemBehavior
{
    private readonly AbstractLevel level;

    public BallScoreBonus(AbstractLevel level)
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