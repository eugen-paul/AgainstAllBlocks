using Godot;
using System;
using System.Diagnostics;

public partial class StartGame : CanvasLayer
{

    public override void _Ready()
    {
        //Just for Test. Don't need it there
        ResourceLoader.LoadThreadedRequest(GamePaths.MAIN_SCENE);
    }

    public override void _Process(double delta)
    {
        Godot.Collections.Array progress = new();
        var status = ResourceLoader.LoadThreadedGetStatus(GamePaths.MAIN_SCENE, progress);

        switch (status)
        {
            case ResourceLoader.ThreadLoadStatus.Loaded:
                var menu = (PackedScene)ResourceLoader.LoadThreadedGet(GamePaths.MAIN_SCENE);
                GetTree().ChangeSceneToPacked(menu);
                break;
            case ResourceLoader.ThreadLoadStatus.InProgress:
                var progressBar = GetNode<ProgressBar>("ColorRect/VBoxContainer/ProgressBar");
                Debug.Print(progress.ToString());
                progressBar.Value = progress[0].AsDouble() * 100;
                break;
            default:
                break;
        }
    }
}
