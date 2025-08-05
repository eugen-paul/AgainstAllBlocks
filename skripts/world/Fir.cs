using Godot;

[Tool]
public partial class Fir : Node3D
{

    private PackedScene _parkScene = ResourceLoader.Load<PackedScene>("res://scenes/world/FirPart.tscn");

    [Export]
    public PackedScene ParkScene
    {
        get => _parkScene;
        set
        {
            _parkScene = value;
            Redraw();
        }
    }

    private int _partsCount = 5;
    [Export]
    public int PartsCount
    {
        get => _partsCount;
        set
        {
            _partsCount = value;
            Redraw();
        }
    }

    private FirType _type = FirType.GREEN;
    [Export]
    public FirType Type
    {
        get => _type;
        set
        {
            _type = value;
            Redraw();
        }
    }

    private RandomNumberGenerator _random = new();

    private int _seed = 0;
    [Export]
    public int Seed
    {
        get => _seed;
        set
        {
            _seed = value;
            _random.Seed = (ulong)_seed;
            Redraw();
        }
    }

    public override void _Ready()
    {
        Redraw();
    }

    private void Redraw()
    {
        // Clear existing children
        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }

        var posYDiff = 0.5f;
        var posY = 0f;
        // Instantiate and add FirPart nodes
        for (int i = 0; i < _partsCount; i++)
        {
            FirPart part = _parkScene.Instantiate<FirPart>();
            part.Type = _type;
            part.Name = "FirPart_" + i;
            part.Position = new Vector3(0, posY, 0);
            posY += posYDiff;
            posYDiff *= 0.85f;

            if (i != 0)
            {
                var scale = Mathf.Pow(0.8f, i);
                part.Scale = new Vector3(scale, scale, scale);
            }
            part.Rotation = new Vector3(0, _random.RandfRange(-Mathf.Pi, Mathf.Pi), 0);
            AddChild(part);
        }
    }

}
