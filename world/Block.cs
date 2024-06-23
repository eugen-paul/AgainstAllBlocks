using System;
using Godot;

public enum BlockColor
{
    YELLOW = 0,
    BLUE
}

[Tool]
public partial class Block : CharacterBody3D
{
    [Export(PropertyHint.Range, "1,10000,")]
    public int MaxPower { get; set; } = 1;

    [Export(PropertyHint.Range, "1,10000,")]
    public int CurrentPower { get; set; } = 1;

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

    public void Hit(int scoreBonus, int hitPower = 1)
    {
        CurrentPower -= hitPower;
        if (CurrentPower <= 0)
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
        var left = Math.Min(CurrentPower, 5);
        if (MaxPower == CurrentPower)
        {
            left = 5;
        }

        StandardMaterial3D mat = _color switch
        {
            BlockColor.YELLOW => GD.Load<StandardMaterial3D>("res://assets/textures/block/yellowMaterial" + left + "Left.tres"),
            BlockColor.BLUE => GD.Load<StandardMaterial3D>("res://assets/textures/block/blueMaterial" + left + "Left.tres"),
            _ => throw new ArgumentException("Illegal Color Value: " + _color.ToString()),
        };
        GetNode<MeshInstance3D>("CollisionShape3D/block/Cube").SetSurfaceOverrideMaterial(0, mat);
    }
}
