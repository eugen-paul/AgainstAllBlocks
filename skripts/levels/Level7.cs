using System;
using Godot;

public partial class Level7 : DefaultLevel
{
    private const string PATH3D_PATH_1 = "Background/Path3D1";
    private const string PATHFOLLOW3D_PATH_1 = PATH3D_PATH_1 + "/PathFollow3D";
    private const string CAR_PATH_1 = PATHFOLLOW3D_PATH_1 + "/Car";

    private const string PATH3D_PATH_2 = "Background/Path3D2";
    private const string PATHFOLLOW3D_PATH_2 = PATH3D_PATH_2 + "/PathFollow3D";
    private const string CAR_PATH_2 = PATHFOLLOW3D_PATH_2 + "/Car";

    private readonly Random random = new();

    public override void _Ready()
    {
        base._Ready();
        GetNodeOrNull<Car>(CAR_PATH_1)?.SetRandomColor();
        GetNodeOrNull<Car>(CAR_PATH_2)?.SetRandomColor();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        MoveOnPath(PATH3D_PATH_1, PATHFOLLOW3D_PATH_1, CAR_PATH_1, (float)(random.NextDouble() * 5 + 10));
        MoveOnPath(PATH3D_PATH_2, PATHFOLLOW3D_PATH_2, CAR_PATH_2, (float)(random.NextDouble() * 5 + 10));
    }

    private void MoveOnPath(string path3DPath, string pathFollow3DPath, string carPath, float speed)
    {
        var path = GetNodeOrNull<Path3D>(path3DPath);
        var pathFollow = GetNodeOrNull<PathFollow3D>(pathFollow3DPath);
        var car = GetNodeOrNull<Car>(carPath);
        if (path != null && pathFollow != null && car != null)
        {
            pathFollow.Progress += (float)GetProcessDeltaTime() * speed;
            if (pathFollow.Progress >= path.GetCurve().GetBakedLength())
            {
                pathFollow.Progress = (float)(random.NextDouble() * 30);
                car.SetRandomColor();
            }
        }
    }
}
