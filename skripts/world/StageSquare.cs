using System;
using System.Diagnostics;
using Godot;
using Godot.Collections;

[Tool]
public partial class StageSquare : StaticBody3D, Hitable
{

    [Export]
    private int MaxXSpeed { get; set; } = 6;

    [Export]
    private int DecelerationX { get; set; } = 2;

    [Export]
    public PackedScene ItemScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_ITEM_SCENE);

    [Export]
    public float LoadingTime { get; set; } = 5;

    private double restTime = 0;

    private bool itemReady = true;

    public Array<ItemType> workingItems = new();

    public Array<ItemType> _items = new();

    [Export]
    public Array<ItemType> Items
    {
        get => _items;
        set
        {
            _items = value;
            workingItems = new Array<ItemType>(_items);
            ShowItem();
        }
    }

    public AbstractLevel Level;

    private readonly Random random = new();

    public override void _Ready()
    {
        SetReady(itemReady);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (restTime > 0)
        {
            restTime = Mathf.Max(0, restTime - delta);
        }
        else if (!itemReady)
        {
            SetReady(true);
        }
    }

    private void SetReady(bool status)
    {
        itemReady = status;
        if (itemReady)
        {
            GetNode<Node3D>("Status").Show();
        }
        else
        {
            GetNode<Node3D>("Status").Hide();
        }
    }

    private void ShowItem()
    {
        if (workingItems.Count == 0 || workingItems[0] == ItemType.NONE)
        {
            GetNode<Sprite3D>("Sprite3D").Hide();
            GetNode<Sprite3D>("Sprite3D").Texture = null;
        }
        else
        {
            GetNode<Sprite3D>("Sprite3D").Show();
            var texture = GD.Load<Texture2D>(ItemBehaviorFactory.GetIconPath(workingItems[0]));
            GetNode<Sprite3D>("Sprite3D").Texture = texture;
        }
    }

    public void Hit(int scoreBonus, int hitPower = 1)
    {
        if (workingItems.Count == 0 || workingItems[0] == ItemType.NONE || !itemReady)
        {
            return;
        }

        var item = ItemScene.Instantiate<Item>();
        item.ItemSpeedX = random.Next(MaxXSpeed);
        if (random.Next(0, 2) == 1)
        {
            item.ItemSpeedX *= -1;
        }
        item.ItemDecelerationX = DecelerationX;
        item.Level = Level;
        item.ItemType = workingItems[0];
        item.Position = GlobalPosition + new Vector3(0, 0.5f, 0);
        Level.TemporaryAdd(item);
        Level.AddChild(item);

        workingItems.Add(workingItems[0]);
        workingItems.RemoveAt(0);
        ShowItem();
        restTime = LoadingTime;
        SetReady(false);
    }
}
