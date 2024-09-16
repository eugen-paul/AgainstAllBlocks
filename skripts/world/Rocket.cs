using Godot;

public partial class Rocket : CharacterBody3D
{
    [Export]
    public float StartSpeed { get; set; } = 22.0f;

    [Export]
    public int ScoreBonus { get; set; } = 20;

    [Export]
    public int Power { get; set; } = 5;

    [Export]
    public PackedScene ExplosionScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_ROCKET_EXPLOSION_SCENE);

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
                SpawnExplosion(collision.GetPosition());
                Level.TemporaryDestroyd(this);
                QueueFree();
            }
            else if (node.IsInGroup("Block"))
            {
                SpawnExplosion(collision.GetPosition());
                if (node is ABlock block)
                {
                    block.Hit(ScoreBonus, Power);
                }
                Level.TemporaryDestroyd(this);
                QueueFree();
            }
        }
    }

    private void SpawnExplosion(Vector3 pos)
    {
        var explosion = ExplosionScene.Instantiate<Explosion>();
        explosion.Level = Level;
        explosion.Position = pos;
        Level.AddChild(explosion);
        Level.TemporaryAdd(explosion);
        explosion.Explode();
    }

    private static Vector3 RemoveY(Vector3 data)
    {
        return new Vector3(data.X, 0, data.Z);
    }

    private void OnVisibilityNotifierScreenExited()
    {
        Level.TemporaryDestroyd(this);
        QueueFree();
    }
}
