using System.Collections.Generic;
using System.Diagnostics;
using Godot;

public partial class SceneLoader : CanvasLayer
{
    private readonly List<string> pathsToLoad = new() {
            GameScenePaths.MAIN_SCENE,
            GameScenePaths.DEFAULT_ARROW_SCENE,
            GameScenePaths.DEFAULT_BALL_SCENE,
            GameScenePaths.DEFAULT_ITEM_SCENE,
            GameScenePaths.DEFAULT_PADDLE_SCENE,
            GameScenePaths.DEFAULT_ROCKET_SCENE,
            GameScenePaths.DEFAULT_ROCKET_EXPLOSION_SCENE,
            GameScenePaths.DEFAULT_BOMB_SCENE,
            GameScenePaths.DEFAULT_BOMB_EXPLOSION_SCENE,
        };

    private List<string> toLoad;

    private string NextScene = GameScenePaths.MAIN_SCENE;

    public override void _Ready()
    {
        toLoad = new(pathsToLoad);
        if (toLoad.Count != 0)
        {
            ResourceLoader.LoadThreadedRequest(pathsToLoad[0]);
        }
    }

    private void GoToNext()
    {
        var next = (PackedScene)ResourceLoader.LoadThreadedGet(NextScene);
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
    }

    public override void _Process(double delta)
    {
        if (toLoad.Count == 0)
        {
            GoToNext();
            return;
        }

        var status = ResourceLoader.LoadThreadedGetStatus(toLoad[0]);
        switch (status)
        {
            case ResourceLoader.ThreadLoadStatus.Loaded:
                // Debug.Print($"Loaded, path = {toLoad[0]}");
                toLoad.RemoveAt(0);
                if (toLoad.Count != 0) { ResourceLoader.LoadThreadedRequest(toLoad[0]); };
                break;
            case ResourceLoader.ThreadLoadStatus.InProgress:
                // Debug.Print($"InProgress, path = {path}");
                break;
            default:
                Debug.Print($"LoadThreadedGetStatus not OK, status = {status}, path = {toLoad[0]}");
                break;
        }

        if (toLoad.Count == 0)
        {
            var next = (PackedScene)ResourceLoader.LoadThreadedGet(NextScene);
            GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
        }
        else
        {
            var progressBar = GetNode<ProgressBar>("ColorRect/VBoxContainer/ProgressBar");
            progressBar.Value = Mathf.Min(100f, 100f * (pathsToLoad.Count - toLoad.Count) / pathsToLoad.Count);
        }
    }
}
