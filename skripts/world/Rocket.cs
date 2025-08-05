using Godot;

public partial class Rocket : CharacterBody3D, AutoSound
{
    [Export]
    public float StartSpeed { get; set; } = 22.0f;

    [Export]
    public int ScoreBonus { get; set; } = 20;

    [Export]
    public int Power { get; set; } = 5;

    [Export]
    public PackedScene ExplosionScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_ROCKET_EXPLOSION_SCENE);

    public DefaultLevel Level { get; set; } = null;

    public void SetTarget(Vector3 targetPosition)
    {
        LookAt(targetPosition, Vector3.Up);
        Velocity = RemoveY(Vector3.Forward.Rotated(Vector3.Up, Rotation.Y) * StartSpeed);
    }

    public override void _Ready()
    {
        var userPreferences = GameComponets.Instance.Get<UserPreferences>();
        if (userPreferences.GetParamEffects() == EffectsPreferences.OFF)
        {
            GetNode<GpuParticles3D>("GPUParticles3D").Emitting = false;
        }
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
                if (node is Hitable hitable)
                {
                    hitable.Hit(ScoreBonus, Power);
                }
                SpawnExplosion(collision.GetPosition());
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

    public void stopSound()
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Autoplay = false;
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
    }

}
