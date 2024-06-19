using System.Diagnostics;
using Godot;

public partial class Arrow : Node3D
{
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Debug.Print(Rotation.Y.ToString());
	}
}
