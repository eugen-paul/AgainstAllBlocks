using Godot;

public abstract partial class Explosion : Node3D
{

    public DefaultLevel Level { get; set; } = null;

    public float Explosionradius { get; set; } = 1f;

    public abstract void Explode(bool sound = true);
}