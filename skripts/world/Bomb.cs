using Godot;
using Godot.Collections;

public partial class Bomb : CharacterBody3D
{
    [Export]
    public float StartSpeed { get; set; } = 22.0f;

    [Export]
    public float StartHeight { get; set; } = 22.0f;

    [Export]
    public float Explosionradius { get; set; } = 3.0f;

    [Export]
    public int ScoreBonus { get; set; } = 20;

    public AbstractLevel Level { get; set; } = null;

    public void SetTarget(Vector3 targetPosition)
    {
        Position = new Vector3(targetPosition.X, StartHeight, targetPosition.Z);
        Velocity = Vector3.Down * StartSpeed;
    }

    public override void _PhysicsProcess(double delta)
    {
        var collision = MoveAndCollide(Velocity * (float)delta);
        if (collision != null && collision.GetCollider() is Node node)
        {
            if (node.IsInGroup("Wall") || node.IsInGroup("Ground") || node.IsInGroup("Block"))
            {
                DoHit();
                Level.TemporaryDestroyd(this);
                QueueFree();
            }
        }
    }

    private void DoHit()
    {
        var hits = new Array<Node>();
        foreach (var node in Level.GetBlocks())
        {
            if (node is Block block)
            {
                if (GlobalPosition.DistanceTo(block.GlobalPosition) <= Explosionradius)
                {
                    hits.Add(block);
                }
            }
        }

        foreach (var node in hits)
        {
            if (node is Block block)
            {
                block.Hit(ScoreBonus);
            }
        }
    }
}