using System.Collections.Generic;
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

    private string NextScene = GameScenePaths.MAIN_SCENE;

    public override void _Ready()
    {
        foreach (var path in pathsToLoad)
        {
            ResourceLoader.LoadThreadedRequest(path);
        }
    }

    public override void _Process(double delta)
    {
        double fullProgress = 0;
        int loaded = 0;

        foreach (var path in pathsToLoad)
        {
            Godot.Collections.Array progress = new();
            var status = ResourceLoader.LoadThreadedGetStatus(path, progress);
            fullProgress += progress[0].AsDouble() * 100;
            switch (status)
            {
                case ResourceLoader.ThreadLoadStatus.Loaded:
                    loaded++;
                    break;
                default:
                    break;
            }
        }

        if (loaded == pathsToLoad.Count)
        {
            var next = (PackedScene)ResourceLoader.LoadThreadedGet(NextScene);
            GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
        }
        else
        {
            var progressBar = GetNode<ProgressBar>("ColorRect/VBoxContainer/ProgressBar");
            progressBar.Value = Mathf.Min(100f, fullProgress / pathsToLoad.Count);
        }
    }
}
