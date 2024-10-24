using Godot;
using System;
using System.Linq;

public partial class GameSlotSave : GameSlot
{

    private readonly static string DELETE_RECT = "DeleteTextureRect";

    private GameProgressData gameProgressData = null;
    private Action loadGameCallback = null;
    private Action deleteGameCallback = null;

    public override void _Ready()
    {
        var levelLabel = GetNode<GameSlotLevelLabel>("BgTextureRect/LevelLabel");
        levelLabel.SetLevel(gameProgressData.Levels.Max(e => e.Value.Reached ? e.Value.Level : 0));

        GetNode<ColorRect>(DELETE_RECT).Hide();
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

    private void OnDeleteButtonPressed()
    {
        GetNode<ColorRect>(DELETE_RECT).Show();
    }

    private void OnDeleteYesButtonPressed()
    {
        GetNode<ColorRect>(DELETE_RECT).Hide();
        deleteGameCallback.Invoke();
    }

    private void OnDeleteNoButtonPressed()
    {
        GetNode<ColorRect>(DELETE_RECT).Hide();
    }
}
