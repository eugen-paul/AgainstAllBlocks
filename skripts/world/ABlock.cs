
using System;
using Godot;

public abstract partial class ABlock : CharacterBody3D, Hitable, ContainItem
{
    protected int _power = 1;

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

    public void Hit(int scoreBonus, int hitPower = 1)
    {
        Power -= hitPower;
        if (Power <= 0)
        {
            InvokeBlockDestroyed(scoreBonus, this);
            QueueFree();
        }
        else
        {
            EditDamageEffect();
        }
    }

    protected abstract void EditDamageEffect();

    protected void InvokeBlockDestroyed(int a, ContainItem b)
    {
        BlockDestroyed.Invoke(a, b);
    }

    public Item CreateItem(DefaultLevel level)
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


    protected void ShowItem()
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
                var texture = GD.Load<Texture2D>(ItemBehaviorFactory.GetIconPath(ItemType));
                GetNode<Sprite3D>("Sprite3D").Texture = texture;
            }
        }
    }
}