using System;
using Godot;

public enum FirType
{
    GREEN = 0,
    SNOW,
}

[Tool]
public partial class FirPart : Node3D
{

    private FirType _type = FirType.GREEN;

    [Export]
    public FirType Type
    {
        get => _type;
        set
        {
            _type = value;
            EditType();
        }
    }

    public override void _Ready()
    {
        EditType();
    }

    private void EditType()
    {
        var mat = _type switch
        {
            FirType.GREEN => GD.Load<StandardMaterial3D>("res://assets/textures/world/Fir-GREEN.tres"),
            FirType.SNOW => GD.Load<StandardMaterial3D>("res://assets/textures/world/Fir-SNOW.tres"),
            _ => throw new ArgumentException("Illegal Fir Type Value: " + _type.ToString()),
        };

        if (GetNode<MeshInstance3D>("fir_1/Cone").GetSurfaceOverrideMaterialCount() > 0)
        {
            GetNode<MeshInstance3D>("fir_1/Cone").SetSurfaceOverrideMaterial(0, mat);
        }
    }
}
