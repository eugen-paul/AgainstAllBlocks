
public partial class Level2 : AbstractLevel
{
    public override void _Ready()
    {
        base._Ready();
    }

    protected override int GetLevel()
    {
        return 2;
    }

    protected override bool GetBall1()
    {
        return false;
    }

    protected override bool GetBall2()
    {
        return false;
    }

    protected override bool GetBall3()
    {
        return false;
    }

}
