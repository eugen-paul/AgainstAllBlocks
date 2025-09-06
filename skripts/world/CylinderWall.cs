using Godot;

public partial class CylinderWall : StaticBody3D, Hitable
{
    public void Hit(int scoreBonus, int hitPower = 1)
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }

}
