using System;
using Godot;

public partial class CaslteDoor : Node3D
{
    private const float TIME_TO_OPEN = 2.0f;
    private const float TIME_TO_CLOSE = 2.0f;

    private const float MOVING_DEG = 90.0f;
    private const float CLOSE_ANGLE_DEG = 0.0f;
    private const float OPEN_ANGLE_DEG = CLOSE_ANGLE_DEG + MOVING_DEG;

    private const string CHAIN_1_PATH = "Chain1";
    private const string CHAIN_1_START_PATH = "DoorNode/Chain1Start";
    private const string CHAIN_1_END_PATH = "Chain1End";

    private const string CHAIN_2_PATH = "Chain2";
    private const string CHAIN_2_START_PATH = "DoorNode/Chain2Start";
    private const string CHAIN_2_END_PATH = "Chain2End";

    private Vector3 chain1StartPos;

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
        else if (doClosing)
        {
            DoClose(delta);
        }
    }

    public void Open()
    {
        doOpening = true;
        doClosing = false;
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }

    public void Close()
    {
        doClosing = true;
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }

    private void DoOpen(double delta)
    {
        var door = GetNode<Node3D>("DoorNode");
        door.RotationDegrees = new Vector3(
            Mathf.Clamp(door.RotationDegrees.X + (float)(MOVING_DEG / TIME_TO_OPEN * delta), CLOSE_ANGLE_DEG, OPEN_ANGLE_DEG),
            door.RotationDegrees.Y,
            door.RotationDegrees.Z
        );
        
        if (door.RotationDegrees.X >= OPEN_ANGLE_DEG)
        {
            doOpening = false;
            openningDone?.Invoke();
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
        }
        else
        {
            ScaleChain(GetNode<Node3D>(CHAIN_1_PATH),
                       GetNode<Node3D>(CHAIN_1_START_PATH),
                       GetNode<Node3D>(CHAIN_1_END_PATH));
            ScaleChain(GetNode<Node3D>(CHAIN_2_PATH),
                       GetNode<Node3D>(CHAIN_2_START_PATH),
                       GetNode<Node3D>(CHAIN_2_END_PATH));
        }
    }

    private void DoClose(double delta)
    {
        var door = GetNode<Node3D>("DoorNode");
        door.RotationDegrees = new Vector3(
            Mathf.Clamp(door.RotationDegrees.X - (float)(MOVING_DEG / TIME_TO_CLOSE * delta), CLOSE_ANGLE_DEG, OPEN_ANGLE_DEG),
            door.RotationDegrees.Y,
            door.RotationDegrees.Z
        );

        if (door.RotationDegrees.X <= CLOSE_ANGLE_DEG)
        {
            doClosing = false;
            closingDone?.Invoke();
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
        }
        else
        {
            ScaleChain(GetNode<Node3D>(CHAIN_1_PATH),
                       GetNode<Node3D>(CHAIN_1_START_PATH),
                       GetNode<Node3D>(CHAIN_1_END_PATH));
            ScaleChain(GetNode<Node3D>(CHAIN_2_PATH),
                       GetNode<Node3D>(CHAIN_2_START_PATH),
                       GetNode<Node3D>(CHAIN_2_END_PATH));
        }
    }

    private static void ScaleChain(Node3D chain, Node3D chainStart, Node3D chainEnd)
    {
        var chainLength = chainEnd.GlobalPosition.DistanceTo(chainStart.GlobalPosition);
        var chainPosition = chainStart.GlobalPosition + (chainEnd.GlobalPosition - chainStart.GlobalPosition).Normalized() * chainLength / 2;
        chain.GlobalPosition = chainPosition;
        chain.LookAt(chainEnd.GlobalPosition, Vector3.Up);
        chain.Scale = new Vector3(1, 1, chainLength);
    }
}
