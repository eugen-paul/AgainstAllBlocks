using System;
using System.Diagnostics;

public partial class GameSlotEmpty : GameSlot
{

    private Action newGameCallback = null;

    public override void _Ready()
    {
        Show();
    }

    public void Init(Action newGameCallback)
    {
        this.newGameCallback = newGameCallback;
    }

    public void OnNewGamePressed()
    {
        newGameCallback();
    }

}
