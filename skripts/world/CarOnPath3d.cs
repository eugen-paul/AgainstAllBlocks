using System.Diagnostics;
using Godot;

public partial class CarOnPath3d : Node3D
{
    private const string PATH3D_PATH = "CarOnPath3d";
    private const string PATHFOLLOW3D_PATH = "CarOnPath3d/PathFollow3D";
    private const string CAR_PATH = "CarOnPath3d/PathFollow3D/Car";
    private const string COLLISION_BODY_PATH = "CarOnPath3d/PathFollow3D/StaticBody3D";

    [Export] private float speed = 10.0f;

    public override void _Ready()
    {
        base._Ready();
        GetNodeOrNull<Car>(CAR_PATH)?.SetRandomColor();
        GetNodeOrNull<CarOnPath3dBody>(COLLISION_BODY_PATH)?.SetChangeDirectionCallback(ResetMoving);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        MoveCarOnPath(delta);
    }

    private void ResetMoving()
    {
        var pathFollow = GetNodeOrNull<PathFollow3D>(PATHFOLLOW3D_PATH);
        pathFollow?.SetProgress(0);
        
        var car = GetNodeOrNull<Car>(CAR_PATH);
        car?.SetRandomColor();
    }

    private void MoveCarOnPath(double delta)
    {
        var path = GetNodeOrNull<Path3D>(PATH3D_PATH);
        var pathFollow = GetNodeOrNull<PathFollow3D>(PATHFOLLOW3D_PATH);
        var car = GetNodeOrNull<Car>(CAR_PATH);
        if (path!= null && pathFollow != null && car != null)
        {
            pathFollow.Progress += (float)(delta * speed);
            if (pathFollow.Progress >= path.GetCurve().GetBakedLength())
            {
                ResetMoving();
            }
        }
    }
}
