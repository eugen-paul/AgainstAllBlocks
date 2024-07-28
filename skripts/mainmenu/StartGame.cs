using Godot;

public partial class StartGame : CanvasLayer
{

    public override void _Ready()
    {
        ResourceLoader.LoadThreadedRequest(GameScenePaths.MAIN_SCENE);
    }

    public override void _Process(double delta)
    {
        Godot.Collections.Array progress = new();
        var status = ResourceLoader.LoadThreadedGetStatus(GameScenePaths.MAIN_SCENE, progress);

        switch (status)
        {
            case ResourceLoader.ThreadLoadStatus.Loaded:
                var menu = (PackedScene)ResourceLoader.LoadThreadedGet(GameScenePaths.MAIN_SCENE);
                GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, menu);
                break;
            case ResourceLoader.ThreadLoadStatus.InProgress:
                var progressBar = GetNode<ProgressBar>("ColorRect/VBoxContainer/ProgressBar");
                progressBar.Value = progress[0].AsDouble() * 100;
                break;
            default:
                break;
        }
    }
}
