using Godot;

public partial class BombExplosion : Explosion
{

    private int done = 4;

    public override void Explode(bool sound = true)
    {
        var userPreferences = GameComponets.Instance.Get<UserPreferences>();

        switch (userPreferences.GetParamEffects())
        {
            case EffectsPreferences.HIGH:
                GetNode<GpuParticles3D>("Debris").Emitting = true;
                GetNode<GpuParticles3D>("Fire").Emitting = true;
                GetNode<GpuParticles3D>("Smoke").Emitting = true;
                done = 4;
                break;
            case EffectsPreferences.LOW:
                GetNode<GpuParticles3D>("Debris").Emitting = true;
                GetNode<GpuParticles3D>("Fire").Emitting = true;
                done = 3;
                break;
            case EffectsPreferences.OFF:
            default:
                done = 1;
                break;
        }

        if (sound)
        {
            GetNode<AudioStreamPlayer>("Sound").Play();
        }
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
