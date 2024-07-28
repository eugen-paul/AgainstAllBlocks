using Godot;
using System;

public partial class GameSlotSave : GameSlot
{

    private GameProgressData gameProgressData = null;
    private Action loadGameCallback = null;
    private Action deleteGameCallback = null;

    public override void _Ready()
    {

    }

    public void Init(GameProgressData gameProgressData, Action loadGameCallback, Action deleteGameCallback)
    {
        this.gameProgressData = gameProgressData;
        this.loadGameCallback = loadGameCallback;
        this.deleteGameCallback = deleteGameCallback;
    }

}
