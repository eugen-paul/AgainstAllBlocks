using Godot;

[Tool]
public partial class Firework : Node2D
{
    private readonly string ExplosionPath = "Explosion";
    private readonly string RocketPath = "Rocket";

    [Export]
    private float VelocityX { get; set; } = 5f;

    [Export]
    private float VelocityY { get; set; } = -20f;

    private bool fired = false;

    public override void _PhysicsProcess(double delta)
    {
        if (!fired)
        {
            return;
        }

        var rocket = GetNode<GpuParticles2D>(RocketPath);
        rocket.GlobalPosition = new Vector2(rocket.GlobalPosition.X + VelocityX * (float)delta, rocket.GlobalPosition.Y + VelocityY * (float)delta);
    }

    public void Fire()
    {
        var rocket = GetNode<GpuParticles2D>(RocketPath);
        rocket.GlobalPosition = GlobalPosition;
        rocket.Emitting = true;
        fired = true;

        GetNode<AudioStreamPlayer>("rocketSound").Play();
    }

    private void RocketEnd()
    {
        var explosion = GetNode<GpuParticles2D>(ExplosionPath);
        var rocket = GetNode<GpuParticles2D>(RocketPath);
        explosion.GlobalPosition = rocket.GlobalPosition;
        explosion.Emitting = true;
        fired = false;

        GetNode<AudioStreamPlayer>("explosionSound").Play();
    }

    private void ExplosionEnd()
    {
    }
}
