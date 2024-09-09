using Godot;

public partial class RocketFun : MeshInstance3D
{

    public override void _PhysicsProcess(double delta)
    {
        Rotation = new Vector3(Rotation.X, Rotation.Y - Mathf.Pi * (float)delta, Rotation.Z);
    }
}
