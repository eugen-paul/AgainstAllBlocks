
public partial class Level1 : AbstractLevel
{
    public override void _Ready()
    {
        base._Ready();
    }

    protected override int GetLevel()
    {
        return 1;
    }

    protected override bool GetBall1()
    {
        return Score >= 75;
    }

    protected override bool GetBall2()
    {
        return Score >= 100;
    }

    protected override bool GetBall3()
    {
        return false;
    }

}
