using Godot;

public partial class Bomb : CharacterBody3D
{
    [Export]
    public float StartSpeed { get; set; } = 22.0f;

    [Export]
    public float StartHeight { get; set; } = 22.0f;

    [Export]
    public int ScoreBonus { get; set; } = 20;

    public AbstractLevel Level { get; set; } = null;

    public void SetTarget(Vector3 targetPosition)
    {
        Position = new Vector3(targetPosition.X, StartHeight, targetPosition.Z - 5);
        Velocity = Vector3.Down * StartSpeed;
    }

    public override void _PhysicsProcess(double delta)
    {
        var collision = MoveAndCollide(Velocity * (float)delta);
        if (collision != null && collision.GetCollider() is Node node)
        {
            if (node.IsInGroup("Wall") || node.IsInGroup("Ground"))
            {
                Level.TemporaryDestroyd(this);
                QueueFree();
            }
            else if (node.IsInGroup("Block"))
            {
                if (node is Block block)
                {
                    block.Hit(ScoreBonus);
                }
                Level.TemporaryDestroyd(this);
                QueueFree();
            }
        }
    }
}
