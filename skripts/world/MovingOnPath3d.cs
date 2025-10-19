using Godot;

public partial class MovingOnPath3d : Path3D
{
    [Export]
    public float Speed { get; set; } = 10.0f;

    [Export]
    public PathFollow3D PathFollower { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        PathFollower.Progress += (float)(delta * Speed);
    }

}
