using System.Collections.Generic;
using System.Diagnostics;
using Godot;

public partial class SceneLoader : Node
{
    private readonly List<string> allPaths = new() {
        GameScenePaths.MAIN_SCENE,
        GameScenePaths.DEFAULT_ARROW_SCENE,
        GameScenePaths.DEFAULT_BALL_SCENE,
        GameScenePaths.DEFAULT_PADDLE_SCENE,
        GameScenePaths.DEFAULT_ROCKET_SCENE,
        GameScenePaths.DEFAULT_ROCKET_EXPLOSION_SCENE,
        GameScenePaths.DEFAULT_BOMB_SCENE,
        GameScenePaths.DEFAULT_BOMB_EXPLOSION_SCENE,
    };

    private readonly List<string> addToSceneItems = new() {
        GameScenePaths.DEFAULT_ITEM_SCENE,
    };

    private readonly List<string> gpuParticles3dList = new() {
        "res://scenes/world/particles/ExplosionDebrisBig.tscn",
        "res://scenes/world/particles/ExplosionDebrisSmall.tscn",
        "res://scenes/world/particles/ExplosionFireBig.tscn",
        "res://scenes/world/particles/ExplosionFireSmall.tscn",
        "res://scenes/world/particles/ExplosionSmokeBig.tscn",
        "res://scenes/world/particles/ExplosionSmokeSmall.tscn",
        "res://scenes/world/particles/RocketBoost.tscn",
    };

    private readonly List<string> gpuParticles2dList = new(){
        "res://scenes/mainmenu/particles/FireworkExplosion.tscn",
        "res://scenes/mainmenu/particles/FireworkRocket.tscn",
    };

    private List<string> pathesToLoad;

    private bool pathsLoaded = false;
    private bool allLoaded = false;
    private int framesAfterLoad = 0;

    private string NextScene = GameScenePaths.MAIN_SCENE;

    public override void _Ready()
    {
        pathesToLoad = new(allPaths);
        if (pathesToLoad.Count != 0)
        {
            ResourceLoader.LoadThreadedRequest(allPaths[0]);
        }

    }

    private void GoToNext()
    {
        var next = (PackedScene)ResourceLoader.LoadThreadedGet(NextScene);
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
    }

    public override void _Process(double delta)
    {
        if (allLoaded)
        {
            framesAfterLoad++;
            if (framesAfterLoad > 3)
            {
                GoToNext();
            }
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if (pathsLoaded)
        {
            LoadAll3DParticles();
            LoadAll2DParticles();
            LoadSprite3D();
            allLoaded = true;
            return;
        }
        else
        {
            LoadAndCheckAllPathes();
        }

    }

    private void LoadAndCheckAllPathes()
    {
        if (pathesToLoad.Count == 0)
        {
            pathsLoaded = true;
            return;
        }

        var status = ResourceLoader.LoadThreadedGetStatus(pathesToLoad[0]);
        switch (status)
        {
            case ResourceLoader.ThreadLoadStatus.Loaded:
                pathesToLoad.RemoveAt(0);
                if (pathesToLoad.Count != 0) { ResourceLoader.LoadThreadedRequest(pathesToLoad[0]); };
                break;
            case ResourceLoader.ThreadLoadStatus.InProgress:
                break;
            default:
                Debug.Print($"LoadThreadedGetStatus not OK, status = {status}, path = {pathesToLoad[0]}");
                break;
        }

        if (pathesToLoad.Count != 0)
        {
            var progressBar = GetNode<ProgressBar>("ColorRect/VBoxContainer/ProgressBar");
            progressBar.Value = Mathf.Min(100f, 100f * (allPaths.Count - pathesToLoad.Count) / allPaths.Count);
        }
    }

    private void LoadAll3DParticles()
    {
        foreach (var particleScene in gpuParticles3dList)
        {
            var scene = ResourceLoader.Load<PackedScene>(particleScene);
            var partocleObj = scene.Instantiate<GpuParticles3D>();
            partocleObj.Emitting = true;
            // partocleObj.Finished += () => { Debug.Print("Finished"); };
            AddChild(partocleObj);
        }
    }

    private void LoadAll2DParticles()
    {
        var bgRect = GetNode<Node>("ParticleRect");
        foreach (var particleScene in gpuParticles2dList)
        {
            var scene = ResourceLoader.Load<PackedScene>(particleScene);
            var partocleObj = scene.Instantiate<GpuParticles2D>();
            partocleObj.Emitting = true;
            // partocleObj.Finished += () => { Debug.Print("Finished"); };
            bgRect.AddChild(partocleObj);
        }
    }

    private void LoadSprite3D()
    {
        foreach (var path in addToSceneItems)
        {
            var itemRl = ResourceLoader.Load<PackedScene>(path);
            var item = itemRl.Instantiate<Node>();
            AddChild(item);
        }
    }

}
