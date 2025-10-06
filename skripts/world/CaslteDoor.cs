using System;
using System.Diagnostics;
using Godot;

public partial class CaslteDoor : Node3D
{
    private const float TIME_TO_OPEN = 2.0f;
    private const float TIME_TO_CLOSE = 2.0f;

    private const float MOVING_DEG = 90.0f;
    private const float CLOSE_ANGLE_DEG = 0.0f;
    private const float OPEN_ANGLE_DEG = CLOSE_ANGLE_DEG + MOVING_DEG;

    private Action openningDone;
    private Action closingDone;

    private bool doOpening = false;
    private bool doClosing = false;

    public void Init(Action openningDone, Action closingDone)
    {
        this.openningDone = openningDone;
        this.closingDone = closingDone;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (doOpening)
        {
            DoOpen(delta);
        }
        if (doClosing)
        {
            DoClose(delta);
        }
    }

    public void Open()
    {
        doOpening = true;
    }

    public void Close()
    {
        doClosing = true;
    }

    private void DoOpen(double delta)
    {
        RotationDegrees = new Vector3(
            Mathf.Clamp(RotationDegrees.X + (float)(MOVING_DEG / TIME_TO_OPEN * delta), CLOSE_ANGLE_DEG, OPEN_ANGLE_DEG),
            RotationDegrees.Y,
            RotationDegrees.Z);
        if (RotationDegrees.X >= OPEN_ANGLE_DEG)
        {
            doOpening = false;
            openningDone?.Invoke();
        }
    }

    private void DoClose(double delta)
    {
        RotationDegrees = new Vector3(
            Mathf.Clamp(RotationDegrees.X - (float)(MOVING_DEG / TIME_TO_CLOSE * delta), CLOSE_ANGLE_DEG, OPEN_ANGLE_DEG),
            RotationDegrees.Y,
            RotationDegrees.Z);
        if (RotationDegrees.X <= CLOSE_ANGLE_DEG)
        {
            doClosing = false;
            closingDone?.Invoke();
        }
    }
}
