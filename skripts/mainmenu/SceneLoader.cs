using System.Collections.Generic;
using Godot;


public partial class SceneLoader : Node
{
    private enum LoadingStage
    {
        SceneItems,
        gpuParticles3dList,
        gpuParticles2dList,
        done,
    }

    private readonly List<string> addToSceneItems = new() {
        GameScenePaths.DEFAULT_ARROW_SCENE,
        GameScenePaths.DEFAULT_BALL_SCENE,
        GameScenePaths.DEFAULT_PADDLE_SCENE,
        GameScenePaths.DEFAULT_ROCKET_SCENE,
        GameScenePaths.DEFAULT_ROCKET_EXPLOSION_SCENE,
        GameScenePaths.DEFAULT_BOMB_SCENE,
        GameScenePaths.DEFAULT_BOMB_EXPLOSION_SCENE,
        GameScenePaths.DEFAULT_ITEM_SCENE,
    };

    private Node lastLoaded = null;

    private readonly List<string> gpuParticles3dList = new() {
        "res://scenes/world/particles/BallFire.tscn",
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
        "res://scenes/mainmenu/particles/FireworkExplosion2.tscn",
        "res://scenes/mainmenu/particles/FireworkRocket.tscn",
        "res://scenes/mainmenu/particles/GoldenBallAnimation.tscn",
        "res://scenes/mainmenu/particles/GoldenBallAchivedAnimation.tscn",
    };

    private List<string> pathesToLoad;

    private bool pathsLoaded = false;
    private int framesAfterLoad = 0;

    private string NextScene = GameScenePaths.MAIN_SCENE;

    private LoadingStage stage = LoadingStage.SceneItems;

    private int itemsCount;

    public override void _Ready()
    {
        pathesToLoad = new(addToSceneItems);
        itemsCount = addToSceneItems.Count + gpuParticles3dList.Count + gpuParticles2dList.Count;
    }

    private void GoToNext()
    {
        var next = ResourceLoader.Load<PackedScene>(GameScenePaths.MAIN_SCENE);
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
    }

    public override void _Process(double delta)
    {
        switch (stage)
        {
            case LoadingStage.SceneItems:
                LoadSprite3D();
                break;
            case LoadingStage.gpuParticles3dList:
                LoadAll3DParticles();
                break;
            case LoadingStage.gpuParticles2dList:
                LoadAll2DParticles();
                break;
            case LoadingStage.done:
            default:
                framesAfterLoad++;
                if (framesAfterLoad > 3)
                {
                    GoToNext();
                }
                break;
        }
    }

    private void LoadAll3DParticles()
    {
        foreach (var particleScene in gpuParticles3dList)
        {
            var scene = ResourceLoader.Load<PackedScene>(particleScene);
            var partocleObj = scene.Instantiate<GpuParticles3D>();
            partocleObj.Emitting = true;
            AddChild(partocleObj);

        }
        var progressBar = GetNode<ProgressBar>("ColorRect/VBoxContainer/ProgressBar");
        progressBar.SetValueNoSignal(Mathf.Min(100f, 100f * (itemsCount - gpuParticles2dList.Count) / itemsCount));
        stage = LoadingStage.gpuParticles2dList;
    }

    private void LoadAll2DParticles()
    {
        var bgRect = GetNode<Node>("ParticleRect");
        foreach (var particleScene in gpuParticles2dList)
        {
            var scene = ResourceLoader.Load<PackedScene>(particleScene);
            var partocleObj = scene.Instantiate<GpuParticles2D>();
            partocleObj.Emitting = true;
            bgRect.AddChild(partocleObj);
        }
        var progressBar = GetNode<ProgressBar>("ColorRect/VBoxContainer/ProgressBar");
        progressBar.SetValueNoSignal(100f);
        stage = LoadingStage.done;
    }

    private void LoadSprite3D()
    {
        lastLoaded?.QueueFree();
        if (stage != LoadingStage.SceneItems)
        {
            return;
        }

        if (pathesToLoad.Count == 0)
        {
            stage = LoadingStage.gpuParticles3dList;
            return;
        }

        lastLoaded = ResourceLoader.Load<PackedScene>(pathesToLoad[0]).Instantiate<Node>();
        if (lastLoaded is Item i)
        {
            i.ItemType = ItemType.LIFE_ADD;
        }
        AddChild(lastLoaded);
        pathesToLoad.RemoveAt(0);

        var progressBar = GetNode<ProgressBar>("ColorRect/VBoxContainer/ProgressBar");
        progressBar.SetValueNoSignal(Mathf.Min(100f, 100f * (itemsCount - pathesToLoad.Count - gpuParticles3dList.Count - gpuParticles2dList.Count) / itemsCount));
    }

}
