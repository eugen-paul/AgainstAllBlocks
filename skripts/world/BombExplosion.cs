using Godot;

public partial class BombExplosion : Explosion
{

    private int done = 5;

    public override void Explode(bool sound = true)
    {
        var userPreferences = GameComponets.Instance.Get<UserPreferences>();

        switch (userPreferences.GetParamEffects())
        {
            case EffectsPreferences.HIGH:
                GetNode<GpuParticles3D>("Debris").Emitting = true;
                GetNode<GpuParticles3D>("Fire").Emitting = true;
                GetNode<GpuParticles3D>("Smoke").Emitting = true;
                GetNode<GpuParticles3D>("Wave").Emitting = true;
                ((PlaneMesh)GetNode<GpuParticles3D>("Wave").DrawPass1).Size = new Vector2(Explosionradius * 3.0f, Explosionradius * 3.0f);
                done = 4;
                break;
            case EffectsPreferences.LOW:
                GetNode<GpuParticles3D>("Debris").Emitting = true;
                GetNode<GpuParticles3D>("Fire").Emitting = true;
                GetNode<GpuParticles3D>("Wave").Emitting = true;
                ((PlaneMesh)GetNode<GpuParticles3D>("Wave").DrawPass1).Size = new Vector2(Explosionradius * 3.0f, Explosionradius * 3.0f);
                done = 3;
                break;
            case EffectsPreferences.OFF:
            default:
                done = 0;
                break;
        }

        if (sound)
        {
            GetNode<AudioStreamPlayer>("Sound").Play();
            done++;
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
