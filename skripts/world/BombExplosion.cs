using Godot;

public partial class BombExplosion : Explosion
{

    private int done = 4;

    public override void Explode()
    {
        GetNode<GpuParticles3D>("Debris").Emitting = true;
        GetNode<GpuParticles3D>("Smoke").Emitting = true;
        GetNode<GpuParticles3D>("Fire").Emitting = true;
        GetNode<AudioStreamPlayer>("Sound").Play();
    }

    public void OnDone()
    {
        done--;
        if (done == 0)
        {
            Level.TemporaryDestroyd(this);
            QueueFree();
        }
    }

}
