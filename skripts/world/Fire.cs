using Godot;

public partial class Fire : Node3D
{
    public override void _Ready()
    {
        var userPreferences = GameComponets.Instance.Get<UserPreferences>();
        if (userPreferences.GetParamEffects() == EffectsPreferences.LOW)
        {
            GetNode<GpuParticles3D>("Flare").Emitting = false;
        }
        else if (userPreferences.GetParamEffects() == EffectsPreferences.OFF)
        {
            GetNode<GpuParticles3D>("Flare").Emitting = false;
            GetNode<GpuParticles3D>("Smoke").Emitting = false;
        }
    }
}
