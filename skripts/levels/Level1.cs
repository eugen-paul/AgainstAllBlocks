
using System;

public partial class Level1 : AbstractLevel
{
    private int rocketCatched;
    private int bombsCatched;

    public override void _Ready()
    {
        base._Ready();
        rocketCatched = 0;
        bombsCatched = 0;
        GameAction += CheckGameSignal;
    }

    protected override int GetLevel()
    {
        return 1;
    }

    protected override bool GetBall1()
    {
        return Score >= 5;
    }

    protected override bool GetBall2()
    {
        return rocketCatched >= 1;
    }

    protected override bool GetBall3()
    {
        return bombsCatched >= 2;
    }

    private void CheckGameSignal(AbstractSignal signal)
    {
        switch (signal)
        {
            case ItemCatchSignal itemSignal:
                ItemCatched(itemSignal);
                break;
        }
    }

    private void ItemCatched(ItemCatchSignal signal)
    {
        switch (signal.Type)
        {
            case ItemType.ROCKET:
                rocketCatched++;
                break;
            case ItemType.BOMBS:
                bombsCatched++;
                break;
        }
    }

}
