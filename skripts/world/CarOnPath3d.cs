using System;
using System.Diagnostics;
using Godot;

public partial class CarOnPath3d : Node3D
{
    private const string PATH3D_PATH = "CarOnPath3d";
    private const string PATHFOLLOW3D_PATH = "CarOnPath3d/PathFollow3D";
    private const string CAR_PATH = "CarOnPath3d/PathFollow3D/Car";
    private const string COLLISION_BODY_PATH = "CarOnPath3d/PathFollow3D/StaticBody3D";

    [Export] private float maxSpeed = 10.0f;
    [Export] private float acceleration = 4.0f;
    [Export] private float waitAfterHitTime = 2.0f;

    private float currentSpeed = 10.0f;
    private float waitingTime = 0.0f;

    public override void _Ready()
    {
        base._Ready();
        InitCarOnPathBody();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        MoveCarOnPath(delta);
    }

    private void HitCar()
    {
        currentSpeed = 0;
        waitingTime = 0;
    }

    private void MoveCarOnPath(double delta)
    {
        var path = GetNodeOrNull<Path3D>(PATH3D_PATH);
        var pathFollow = GetNodeOrNull<PathFollow3D>(PATHFOLLOW3D_PATH);
        var car = GetNodeOrNull<Car>(CAR_PATH);
        if (path != null && pathFollow != null && car != null)
        {
            if (waitingTime < waitAfterHitTime)
            {
                waitingTime += (float)delta;
            }
            else
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed = Math.Min(currentSpeed + (float)(acceleration * delta), maxSpeed);
                }

                pathFollow.Progress += (float)(delta * currentSpeed);
                if (pathFollow.Progress >= path.GetCurve().GetBakedLength())
                {
                    InitCarOnPathBody();
                }
            }
        }
    }

    private void InitCarOnPathBody()
    {
        var carOnPathBody = GetNodeOrNull<CarOnPath3dBody>(COLLISION_BODY_PATH);
        carOnPathBody?.SetHitCarCallback(HitCar);

        GetNodeOrNull<Car>(CAR_PATH)?.SetRandomColor();

        var pathFollow = GetNodeOrNull<PathFollow3D>(PATHFOLLOW3D_PATH);
        if (pathFollow != null)
        {
            pathFollow.Progress = 0;
        }

        currentSpeed = maxSpeed;
        waitingTime = waitAfterHitTime;
    }
}
