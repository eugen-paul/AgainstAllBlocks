using Godot;
using System;
using System.Diagnostics;

public partial class Block : CharacterBody3D
{

    [Export]
    public int MaxPower { get; set; } = 1;

    [Export]
    public Texture2D PowerLeft4 { get; set; } = GD.Load<Texture2D>("res://assets/textures/PowerLeft4.png");
    [Export]
    public Texture2D PowerLeft3 { get; set; } = GD.Load<Texture2D>("res://assets/textures/PowerLeft3.png");
    [Export]
    public Texture2D PowerLeft2 { get; set; } = GD.Load<Texture2D>("res://assets/textures/PowerLeft2.png");
    [Export]
    public Texture2D PowerLeft1 { get; set; } = GD.Load<Texture2D>("res://assets/textures/PowerLeft1.png");

    private int _currentPower;

    private StandardMaterial3D _mat;

    [Signal]
    public delegate void BlockDestroyedEventHandler(int scoreBonus = 0);

    public override void _Ready()
    {
        _currentPower = MaxPower;
        var block = GetNode<MeshInstance3D>("CollisionShape3D/block/Cube");
        _mat = (StandardMaterial3D)block.GetSurfaceOverrideMaterial(0).Duplicate();
        block.SetSurfaceOverrideMaterial(0, _mat);
    }

    public void Hit(int scoreBonus, int hitPower = 1)
    {
        _currentPower -= hitPower;
        if (_currentPower <= 0)
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
        _mat.DetailEnabled = true;

        var powerLeft = MaxPower - _currentPower;
        if (powerLeft >= 4)
        {
            _mat.DetailAlbedo = PowerLeft4;
        }
        else if (powerLeft == 3)
        {
            _mat.DetailAlbedo = PowerLeft3;
        }
        else if (powerLeft == 2)
        {
            _mat.DetailAlbedo = PowerLeft2;
        }
        else
        {
            _mat.DetailAlbedo = PowerLeft1;
        }
    }
}
