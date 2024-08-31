using System.Diagnostics;
using Godot;

public partial class Rocket : CharacterBody3D
{
    [Export]
    public float StartSpeed { get; set; } = 35.0f;

    [Export]
    public int ScoreBonus { get; set; } = 20;

    public AbstractLevel Level { get; set; } = null;

    public void SetTarget(Vector3 targetPosition)
    {
        LookAt(targetPosition, Vector3.Up);
        Velocity = RemoveY(Vector3.Forward.Rotated(Vector3.Up, Rotation.Y) * StartSpeed);
    }

    public override void _PhysicsProcess(double delta)
    {
        var collision = MoveAndCollide(Velocity * (float)delta);
        if (collision != null && collision.GetCollider() is Node node)
        {
            if (node.IsInGroup("Wall"))
            {
                Velocity = Velocity.Bounce(collision.GetNormal());
                Level.RocketDestroyd(this);
                QueueFree();
            }
            else if (node.IsInGroup("Block"))
            {
                if (node is Block block)
                {
                    block.Hit(ScoreBonus);
                }
                Level.RocketDestroyd(this);
                QueueFree();
            }
        }
    }

    private static Vector3 RemoveY(Vector3 data)
    {
        return new Vector3(data.X, 0, data.Z);
    }

    private void OnVisibilityNotifierScreenExited()
    {
        Level.RocketDestroyd(this);
        QueueFree();
    }
}
