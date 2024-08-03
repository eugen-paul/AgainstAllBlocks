using Godot;
using System;
using System.Diagnostics;

public partial class GameSlotSave : GameSlot
{

    private GameProgressData gameProgressData = null;
    private Action loadGameCallback = null;
    private Action deleteGameCallback = null;

    public override void _Ready()
    {
        var levelLabel = GetNode<GameSlotLevelLabel>("BgTextureRect/LevelLabel");
        levelLabel.SetLevel(gameProgressData.Levels.Count);
    }

    public void Init(GameProgressData gameProgressData, Action loadGameCallback, Action deleteGameCallback)
    {
        this.gameProgressData = gameProgressData;
        this.loadGameCallback = loadGameCallback;
        this.deleteGameCallback = deleteGameCallback;
    }

    private void OnLoadButtonPressed()
    {
        loadGameCallback.Invoke();
    }
}
