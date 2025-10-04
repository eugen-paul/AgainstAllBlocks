using System;
using Godot;

public partial class Loading : CanvasLayer
{
    private const string PROGRESSBAR_PATH = "BG/CenterContainer/VBoxContainer/ProgressBar";

    private string loadingPath = null;

    public event Action LoadError;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (loadingPath != null)
        {
            Godot.Collections.Array progress = [];
            switch (ResourceLoader.LoadThreadedGetStatus(loadingPath, progress))
            {
                case ResourceLoader.ThreadLoadStatus.InProgress:
                    var progressBar = GetNode<ProgressBar>(PROGRESSBAR_PATH);
                    progressBar.SetValueNoSignal((float)progress[0] * 100f);
                    break;
                case ResourceLoader.ThreadLoadStatus.InvalidResource:
                    GD.PrintErr("InvalidResource: " + loadingPath);
                    loadingPath = null;
                    LoadError?.Invoke();
                    break;
                case ResourceLoader.ThreadLoadStatus.Loaded:
                    var next = ResourceLoader.Load<PackedScene>(loadingPath, cacheMode: ResourceLoader.CacheMode.IgnoreDeep);
                    loadingPath = null;
                    GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
                    break;
                case ResourceLoader.ThreadLoadStatus.Failed:
                    GD.PrintErr("Failed to load scene: " + loadingPath);
                    loadingPath = null;
                    LoadError?.Invoke();
                    break;
            }
        }
    }

    public void LoadLevel(int level)
    {
        loadingPath = GameScenePaths.GetLevelPath(level);
        ResourceLoader.LoadThreadedRequest(loadingPath, cacheMode: ResourceLoader.CacheMode.IgnoreDeep);
    }
}
