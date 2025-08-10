using Godot;
using Godot.Collections;

public enum MovingDirection
{
    Up,
    Down,
    Left,
    Right,
    None
}

public partial class MovingBlock : Node3D
{
    [Export] public float Speed = 5.0f;
    [Export]
    public Array<MovingDirection> directions = [MovingDirection.Right,
                                                MovingDirection.Down,
                                                MovingDirection.Left,
                                                MovingDirection.Up,
                                                ];
    [Export] public float MinX = -10.0f;
    [Export] public float MaxX = 10.0f;
    [Export] public float MinZ = -10.0f;
    [Export] public float MaxZ = 10.0f;

    private int currentDirectionIndex = 0;
    private float aabbMinX = 1.0f;
    private float aabbMaxX = 1.0f;
    private float aabbMinZ = 1.0f;
    private float aabbMaxZ = 1.0f;

    public override void _Ready()
    {
        var blocks = FindChildren("Block*");
        foreach (var block in blocks)
        {
            if (block is ABlock aBlock)
            {
                aBlock.BlockDestroyed += (_, _) =>
                {
                    CallDeferred("ComputeAABB");
                };
            }
        }

        ComputeAABB();
    }

    private void ComputeAABB()
    {
        aabbMinX = float.MaxValue;
        aabbMaxX = float.MinValue;
        aabbMinZ = float.MaxValue;
        aabbMaxZ = float.MinValue;

        // Find all children of type ABlock and merge their AABBs into the global AABB
        var blocks = FindChildren("Block*");
        foreach (var block in blocks)
        {
            if (block.IsQueuedForDeletion())
            {
                continue;
            }
            if (block is ABlock aBlock)
            {
                Aabb aabb = aBlock.GetGlobalAABB();
                aabbMinX = Mathf.Min(aabbMinX, aabb.Position.X);
                aabbMaxX = Mathf.Max(aabbMaxX, aabb.Position.X + aabb.Size.X);
                aabbMinZ = Mathf.Min(aabbMinZ, aabb.Position.Z);
                aabbMaxZ = Mathf.Max(aabbMaxZ, aabb.Position.Z + aabb.Size.Z);
            }
        }
        aabbMinX -= GlobalPosition.X;
        aabbMaxX -= GlobalPosition.X;
        aabbMinZ -= GlobalPosition.Z;
        aabbMaxZ -= GlobalPosition.Z;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        currentDirectionIndex %= directions.Count;// Loop through directions

        MovingDirection direction = directions[currentDirectionIndex];

        switch (direction)
        {
            case MovingDirection.Up:
                MoveUp(delta);
                break;
            case MovingDirection.Down:
                MoveDown(delta);
                break;
            case MovingDirection.Left:
                MoveLeft(delta);
                break;
            case MovingDirection.Right:
                MoveRight(delta);
                break;
        }
    }

    public void MoveRight(double delta)
    {
        var translation = new Vector3(Speed * (float)delta, 0, 0);
        Translate(translation);
        if (Position.X + aabbMaxX >= MaxX)
        {
            Position = new Vector3(MaxX - aabbMaxX, Position.Y, Position.Z);
            currentDirectionIndex++;
        }
    }

    public void MoveLeft(double delta)
    {
        var translation = new Vector3(-Speed * (float)delta, 0, 0);
        Translate(translation);
        if (Position.X + aabbMinX <= MinX)
        {
            Position = new Vector3(MinX - aabbMinX, Position.Y, Position.Z);
            currentDirectionIndex++;
        }
    }

    public void MoveDown(double delta)
    {
        var translation = new Vector3(0, 0, Speed * (float)delta);
        Translate(translation);
        if (Position.Z + aabbMaxZ >= MaxZ)
        {
            Position = new Vector3(Position.X, Position.Y, MaxZ - aabbMaxZ);
            currentDirectionIndex++;
        }
    }

    public void MoveUp(double delta)
    {
        var translation = new Vector3(0, 0, -Speed * (float)delta);
        Translate(translation);
        if (Position.Z + aabbMinZ <= MinZ)
        {
            Position = new Vector3(Position.X, Position.Y, MinZ - aabbMinZ);
            currentDirectionIndex++;
        }
    }
}
