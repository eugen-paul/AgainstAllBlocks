using System;
using Godot;

public enum SmallWallType
{
    BRICK = 0,
    STONE,
    PLANK,
}

[Tool]
public partial class SmallWall : Node3D
{
    private SmallWallType _wallType = SmallWallType.BRICK;
    [Export]
    public SmallWallType WallType
    {
        get => _wallType;
        set
        {
            _wallType = value;
            EditDamageEffect();
        }
    }

    protected void EditDamageEffect()
    {
        StandardMaterial3D mat = _wallType switch
        {
            SmallWallType.BRICK => GD.Load<StandardMaterial3D>("res://assets/textures/world/SmallWall_BRICK.tres"),
            SmallWallType.STONE => GD.Load<StandardMaterial3D>("res://assets/textures/world/SmallWall_STONE.tres"),
            SmallWallType.PLANK => GD.Load<StandardMaterial3D>("res://assets/textures/world/SmallWall_PLANK.tres"),
            _ => throw new ArgumentException("Illegal Color Value: " + _wallType.ToString()),
        };

        if (GetNode<MeshInstance3D>("smallWall/SmalWall").GetSurfaceOverrideMaterialCount() > 0)
        {
            GetNode<MeshInstance3D>("smallWall/SmalWall").SetSurfaceOverrideMaterial(0, mat);
        }
    }

}
