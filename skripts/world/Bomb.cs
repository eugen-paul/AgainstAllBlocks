using Godot;
using Godot.Collections;

public partial class Bomb : CharacterBody3D, AutoSound
{
    [Export]
    public float StartSpeed { get; set; } = 22.0f;

    [Export]
    public float StartHeight { get; set; } = 22.0f;

    [Export]
    public float Explosionradius { get; set; } = 2.0f;

    [Export]
    public int ScoreBonus { get; set; } = 20;

    [Export]
    public PackedScene ExplosionScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_BOMB_EXPLOSION_SCENE);

    public DefaultLevel Level { get; set; } = null;

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
            if (node is ABlock block)
            {
                if (GlobalPosition.DistanceTo(block.GlobalPosition) <= Explosionradius)
                {
                    hits.Add(block);
                }
            }
        }

        foreach (var node in hits)
        {
            if (node is ABlock block)
            {
                block.Hit(ScoreBonus);
            }
        }

        var explosion = ExplosionScene.Instantiate<Explosion>();
        explosion.Level = Level;
        explosion.Position = GlobalPosition;
        explosion.
        Level.AddChild(explosion);
        Level.TemporaryAdd(explosion);
        explosion.Explode();
    }

    public void stopSound()
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Autoplay = false;
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
    }
}
