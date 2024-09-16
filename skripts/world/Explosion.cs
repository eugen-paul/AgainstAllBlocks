using Godot;

public abstract partial class Explosion : Node3D
{

    public AbstractLevel Level { get; set; } = null;

    public abstract void Explode();
}