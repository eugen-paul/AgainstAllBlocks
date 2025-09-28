using Godot;
using Godot.Collections;

public partial class Bomb : CharacterBody3D, AutoSound
{
    [Export]
    public float StartSpeed { get; set; } = 22.0f;

    [Export]
    public float StartHeight { get; set; } = 22.0f;

    private float explosionradius = 2.0f;
    private int explosionpower  = 1;

    private int scoreBonus = 0;

    [Export]
    public PackedScene ExplosionScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_BOMB_EXPLOSION_SCENE);

    public DefaultLevel Level { get; set; } = null;

    public void SetTarget(Vector3 targetPosition)
    {
        Position = new Vector3(targetPosition.X, StartHeight, targetPosition.Z);
        Velocity = Vector3.Down * StartSpeed;
    }

    public override void _Ready()
    {
        if (GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().IsUpgradeTypeActive(UpgradeType.BOMB_RADIUS))
        {
            explosionradius = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentUpgradeLevel(UpgradeType.BOMB_RADIUS) + 2f;
        }
        if (GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().IsUpgradeTypeActive(UpgradeType.BOMB_SCORE))
        {
            scoreBonus = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentUpgradeLevel(UpgradeType.BOMB_SCORE) * 2;
        }
        if (GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().IsUpgradeTypeActive(UpgradeType.BOMB_POWER))
        {
            explosionpower = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentUpgradeLevel(UpgradeType.BOMB_POWER) + 1;
        }
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
                if (GlobalPosition.DistanceTo(block.GlobalPosition) <= explosionradius)
                {
                    hits.Add(block);
                }
            }
        }

        foreach (var node in hits)
        {
            if (node is ABlock block)
            {
                block.Hit(scoreBonus, explosionpower);
            }
        }

        var explosion = ExplosionScene.Instantiate<Explosion>();
        explosion.Level = Level;
        explosion.Position = GlobalPosition;
        explosion.Explosionradius = explosionradius;
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
