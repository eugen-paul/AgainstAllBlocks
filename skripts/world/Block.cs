using System;
using System.Diagnostics;
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
public partial class Block : CharacterBody3D, ContainItem, Hitable
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

    public ItemType _itemType = ItemType.NONE;
    [Export]
    public ItemType ItemType
    {
        get => _itemType;
        set
        {
            _itemType = value;
            ShowItem();
        }
    }

    [Export]
    public PackedScene ItemScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_ITEM_SCENE);

    public event Action<int, ContainItem> BlockDestroyed;

    public override void _Ready()
    {
        EditDamageEffect();
        ShowItem();
    }

    public void Hit(int scoreBonus, int hitPower = 1)
    {
        Power -= hitPower;
        if (Power <= 0)
        {
            BlockDestroyed.Invoke(scoreBonus, this);
            QueueFree();
        }
        else
        {
            EditDamageEffect();
        }
    }

    public Item CreateItem(AbstractLevel level)
    {
        if (ItemType == ItemType.NONE)
        {
            return null;
        }

        var item = ItemScene.Instantiate<Item>();
        item.Level = level;
        item.ItemType = ItemType;
        item.Position = GlobalPosition + new Vector3(0, 0.5f, 0);
        return item;
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

    private void ShowItem()
    {
        if (Engine.IsEditorHint())
        {
            if (ItemType == ItemType.NONE)
            {
                GetNode<Sprite3D>("Sprite3D").Hide();
                GetNode<Sprite3D>("Sprite3D").Texture = null;
            }
            else
            {
                GetNode<Sprite3D>("Sprite3D").Show();
                var texture = GD.Load<Texture2D>(ItemBehaviorFactory.getIconPath(ItemType));
                GetNode<Sprite3D>("Sprite3D").Texture = texture;
            }
        }
    }
}
