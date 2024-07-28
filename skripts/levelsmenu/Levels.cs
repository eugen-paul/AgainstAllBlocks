using Godot;

public partial class Levels : CanvasLayer
{
    private const string LEVELS_CONTAINER_PATH = "Panel/ScrollContainer/VBoxContainer";

    [Export]
    public PackedScene LevelSelectionScene { get; set; }

    public override void _Ready()
    {
        var gameData = GameComponets.Instance.Get<CurrentGame>().Game;

        var lvlContainer = GetNode<VBoxContainer>(LEVELS_CONTAINER_PATH);
        foreach (var child in lvlContainer.GetChildren())
        {
            child.QueueFree();
        }

        for (int i = 1; i < gameData.Levels.Count; i++)
        {
            var lvl = LevelSelectionScene.Instantiate<LevelProgress>();
            lvl.Init(gameData.Levels[i]);
            lvlContainer.AddChild(lvl);
        }
    }
}
