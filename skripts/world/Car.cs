using Godot;
using System;

public enum CarColor
{
    GREEN = 0,
    RED,
    YELLOW,
    GRAY,
    BLUE,
}

[Tool]
public partial class Car : Node3D
{

    private CarColor _color = CarColor.YELLOW;
    [Export]
    public CarColor Color
    {
        get => _color;
        set
        {
            _color = value;
            EditDamageEffect();
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void EditDamageEffect()
    {
        StandardMaterial3D mat = _color switch
        {
            CarColor.YELLOW => GD.Load<StandardMaterial3D>("res://assets/textures/car/yellowCarMaterial.tres"),
            CarColor.BLUE => GD.Load<StandardMaterial3D>("res://assets/textures/car/blueCarMaterial.tres"),
            CarColor.GREEN => GD.Load<StandardMaterial3D>("res://assets/textures/car/greenCarMaterial.tres"),
            CarColor.RED => GD.Load<StandardMaterial3D>("res://assets/textures/car/redCarMaterial.tres"),
            CarColor.GRAY => GD.Load<StandardMaterial3D>("res://assets/textures/car/grayCarMaterial.tres"),
            _ => throw new ArgumentException("Illegal Color Value: " + _color.ToString()),
        };

        if (GetNode<MeshInstance3D>("car/Cube").GetSurfaceOverrideMaterialCount() > 0)
        {
            GetNode<MeshInstance3D>("car/Cube").SetSurfaceOverrideMaterial(0, mat);
        }
        if (GetNode<MeshInstance3D>("car/wheel").GetSurfaceOverrideMaterialCount() > 0)
        {
            GetNode<MeshInstance3D>("car/wheel").SetSurfaceOverrideMaterial(0, mat);
        }
        if (GetNode<MeshInstance3D>("car/wheel_001").GetSurfaceOverrideMaterialCount() > 0)
        {
            GetNode<MeshInstance3D>("car/wheel_001").SetSurfaceOverrideMaterial(0, mat);
        }
        if (GetNode<MeshInstance3D>("car/wheel_002").GetSurfaceOverrideMaterialCount() > 0)
        {
            GetNode<MeshInstance3D>("car/wheel_002").SetSurfaceOverrideMaterial(0, mat);
        }
        if (GetNode<MeshInstance3D>("car/wheel_003").GetSurfaceOverrideMaterialCount() > 0)
        {
            GetNode<MeshInstance3D>("car/wheel_003").SetSurfaceOverrideMaterial(0, mat);
        }
    }
}
