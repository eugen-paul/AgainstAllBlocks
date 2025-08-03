using Godot;

public partial class DynamiteExplosion : Explosion, ITriggerable
{

    private bool _triggered = false;

    public override void Explode(bool sound = true)
    {
        var userPreferences = GameComponets.Instance.Get<UserPreferences>();

        switch (userPreferences.GetParamEffects())
        {
            case EffectsPreferences.HIGH:
                GetNode<GpuParticles3D>("Debris").Emitting = true;
                GetNode<GpuParticles3D>("Fire").Emitting = true;
                GetNode<GpuParticles3D>("Smoke").Emitting = true;
                break;
            case EffectsPreferences.LOW:
                GetNode<GpuParticles3D>("Debris").Emitting = true;
                GetNode<GpuParticles3D>("Fire").Emitting = true;
                break;
            case EffectsPreferences.OFF:
            default:
                break;
        }

        if (sound)
        {
            GetNode<AudioStreamPlayer>("Sound").Play();
        }
    }

    public void OnDone(){}

    public void Trigger()
    {
        if (_triggered)
        {
            return;
        }
        _triggered = true;
        Explode();
    }

}
