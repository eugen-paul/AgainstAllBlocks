using Godot;

public partial class UfoPath3d : Path3D
{
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        GetChild<PathFollow3D>(0).Progress+= (float)(delta * 10);
    }

}
