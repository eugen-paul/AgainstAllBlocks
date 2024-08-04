using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class GameSlotSave : GameSlot
{

    private GameProgressData gameProgressData = null;
    private Action loadGameCallback = null;
    private Action deleteGameCallback = null;

    public override void _Ready()
    {
        var levelLabel = GetNode<GameSlotLevelLabel>("BgTextureRect/LevelLabel");
        levelLabel.SetLevel(gameProgressData.Levels.Max(e => e.Value.Reached ? e.Value.Level : 0));
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
