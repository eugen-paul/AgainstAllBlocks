using Godot;

public partial class Spawner : Path3D
{

    private const string PATHFOLLOW3D_CHILD_PATH = "PathFollow3D";
    private const string CHILDS_TO_SPAWN_PATH = "ToSpawn";

    [Export]
    private double SpawnIntervalMin { get; set; } = 1.0;

    [Export]
    private double SpawnIntervalMax { get; set; } = 2.0;

    [Export]
    private bool RandomSpawnOrder { get; set; } = true;

    private int nextChildToSpawnIndex = 0;

    private double _timeUntilNextSpawn = 0.0;

    public override void _Ready()
    {
        base._Ready();
        ResetSpawnTimer();
    }

    private void ResetSpawnTimer()
    {
        _timeUntilNextSpawn = GD.RandRange(SpawnIntervalMin, SpawnIntervalMax);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        _timeUntilNextSpawn -= delta;
        if (_timeUntilNextSpawn <= 0.0)
        {
            SpawnObject();
            ResetSpawnTimer();
        }
        MoveAllObjectsOnPath(delta);
    }

    private void SpawnObject()
    {
        var toSpawnObjects = GetNodeOrNull<Node>(CHILDS_TO_SPAWN_PATH);
        var pathFollow = GetNodeOrNull<PathFollow3D>(PATHFOLLOW3D_CHILD_PATH);
        if (toSpawnObjects == null || pathFollow == null || toSpawnObjects.GetChildCount() == 0)
        {
            return;
        }

        Node3D objectToSpawn = null;
        if (RandomSpawnOrder)
        {
            var childIndex = (int)GD.Randi() % toSpawnObjects.GetChildCount();
            objectToSpawn = toSpawnObjects.GetChildOrNull<Node3D>(childIndex);
        }
        else
        {
            objectToSpawn = toSpawnObjects.GetChildOrNull<Node3D>(nextChildToSpawnIndex);
        }

        if (objectToSpawn == null)
        {
            return;
        }

        nextChildToSpawnIndex = (nextChildToSpawnIndex + 1) % toSpawnObjects.GetChildCount();
        if (pathFollow.Duplicate() is PathFollow3D newPathFollow && objectToSpawn.Duplicate() is Node3D newObject)
        {
            newPathFollow.AddChild(newObject);
            newObject.Visible = true;
            AddChild(newPathFollow);
            newPathFollow.Name = $"TEMP_PATH_{nextChildToSpawnIndex}";
            newPathFollow.Progress = 0.0f;
        }
    }

    private void MoveAllObjectsOnPath(double delta)
    {
        var childCnt = GetChildCount();
        for (int i = 0; i < childCnt; i++)
        {
            if (GetChildOrNull<PathFollow3D>(i) is PathFollow3D pathFollow && pathFollow.Name.ToString().StartsWith("TEMP_PATH"))
            {
                pathFollow.Progress += (float)(delta * 2.0);
                if (pathFollow.Progress >= GetCurve().GetBakedLength())
                {
                    pathFollow.QueueFree();
                }
            }
        }
    }
}
