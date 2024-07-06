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
public partial class Block : CharacterBody3D
{
    private int _power = 1;

    [Export(PropertyHint.Range, "1,10000,")]
    public int Power
    {
        get => _power;
        set
        {
            _power = value;
            EditDamageEffect();
        }
    }

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

    [Signal]
    public delegate void BlockDestroyedEventHandler(int scoreBonus = 0);

    public override void _Ready()
    {
        EditDamageEffect();
    }

    public void Hit(int scoreBonus, int hitPower = 1)
    {
        Power -= hitPower;
        if (Power <= 0)
        {
            EmitSignal(SignalName.BlockDestroyed, scoreBonus);
            QueueFree();
        }
        else
        {
            EditDamageEffect();
        }
    }

    private void EditDamageEffect()
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
