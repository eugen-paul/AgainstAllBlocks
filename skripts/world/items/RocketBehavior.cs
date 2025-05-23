
using System;
using Godot;

public class RocketBehavior : ItemBehavior
{
    private readonly DefaultLevel level;
    private readonly Item item;

    public PackedScene RocketScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_ROCKET_SCENE);

    public RocketBehavior(Item item, DefaultLevel level)
    {
        this.level = level;
        this.item = item;
    }

    public override void DoBehavior()
    {
        var blocks = level.GetBlocks();
        Random random = new();
        int position = random.Next(0, blocks.Count);
        var blockFromArray = blocks[position];

        if (blockFromArray is ABlock block)
        {
            var rocket = RocketScene.Instantiate<Rocket>();
            rocket.Level = level;
            rocket.Position = item.GlobalPosition;
            level.AddChild(rocket);
            rocket.SetTarget(block.GlobalPosition);
            level.TemporaryAdd(rocket);
        }
    }
}