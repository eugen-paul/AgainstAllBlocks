using System;
using Godot;

public enum BlockColor
{
    YELLOW = 0,
    BLUE,
    GREEN,
    RED,
    PURPLE,
}

[Tool]
public partial class Block : ABlock
{

    private BlockColor _color = BlockColor.YELLOW;
    [Export]
    public BlockColor Color
    {
        get => _color;
        set
        {
            _color = value;
            EditDamageEffect();
        }
    }

    public override void _Ready()
    {
        EditDamageEffect();
        ShowItem();
    }

    protected override void EditDamageEffect()
    {
        var powerLeft = Math.Min(Power, 5);
        powerLeft = Math.Max(powerLeft, 1);

        StandardMaterial3D mat = _color switch
        {
            BlockColor.YELLOW => GD.Load<StandardMaterial3D>("res://assets/textures/block/yellowMaterial" + powerLeft + "Left.tres"),
            BlockColor.BLUE => GD.Load<StandardMaterial3D>("res://assets/textures/block/blueMaterial" + powerLeft + "Left.tres"),
            BlockColor.GREEN => GD.Load<StandardMaterial3D>("res://assets/textures/block/greenMaterial" + powerLeft + "Left.tres"),
            BlockColor.RED => GD.Load<StandardMaterial3D>("res://assets/textures/block/redMaterial" + powerLeft + "Left.tres"),
            BlockColor.PURPLE => GD.Load<StandardMaterial3D>("res://assets/textures/block/purpleMaterial" + powerLeft + "Left.tres"),
            _ => throw new ArgumentException("Illegal Color Value: " + _color.ToString()),
        };
        if (GetNode<MeshInstance3D>("CollisionShape3D/block/Cube").GetSurfaceOverrideMaterialCount() > 0)
        {
            GetNode<MeshInstance3D>("CollisionShape3D/block/Cube").SetSurfaceOverrideMaterial(0, mat);
        }
    }

}
