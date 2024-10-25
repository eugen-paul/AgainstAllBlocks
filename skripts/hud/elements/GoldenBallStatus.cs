using Godot;

public partial class GoldenBallStatus : CenterContainer
{
    public enum BallStatus
    {
        GRAY = 0,
        TO_GOLD = 1,
        GOLD = 2,
    }

    private GoldenBallBox Ball1;
    private GoldenBallBox Ball2;
    private GoldenBallBox Ball3;

    public override void _Ready()
    {
        Ball1 = GetNode<GoldenBallBox>("VBoxContainer/Ball1");
        Ball2 = GetNode<GoldenBallBox>("VBoxContainer/Ball2");
        Ball3 = GetNode<GoldenBallBox>("VBoxContainer/Ball3");
    }

    public void ShowBalls(BallStatus BallStatus1, BallStatus BallStatus2, BallStatus BallStatus3)
    {
        SetBallStatus(Ball1, BallStatus1);
        SetBallStatus(Ball2, BallStatus2);
        SetBallStatus(Ball3, BallStatus3);
    }

    public void SetTooltips(string ball1, string ball2, string ball3)
    {
        Ball1.SetTooltipForBall(ball1);
        Ball2.SetTooltipForBall(ball2);
        Ball3.SetTooltipForBall(ball3);
    }

    private void SetBallStatus(GoldenBallBox Ball, BallStatus Status)
    {
        switch (Status)
        {
            case BallStatus.GRAY:
                Ball.BallType = GoldenBallType.GRAY;
                break;
            case BallStatus.TO_GOLD:
            case BallStatus.GOLD:
                Ball.BallType = GoldenBallType.GOLD;
                break;
            default:
                break;
        }

        if (Status == BallStatus.TO_GOLD)
        {
            Ball.PlayAnimation();
        }
    }
}
