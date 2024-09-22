using Godot;
using System;

public partial class RocketExplosion : Explosion
{
    private int done = 4;

    public override void Explode()
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
