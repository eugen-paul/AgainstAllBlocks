
using System;
using Godot;

public class RocketBehavior : ItemBehavior
{
    private readonly AbstractLevel level;
    private readonly Item item;

    public PackedScene RocketScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_ROCKET_SCENE);

    public RocketBehavior(Item item, AbstractLevel level)
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

        if (blockFromArray is Block block)
        {
            var rocket = RocketScene.Instantiate<Rocket>();
            rocket.Level = level;
            rocket.Position = item.GlobalPosition;
            level.AddChild(rocket);
            rocket.SetTarget(block.GlobalPosition);
            level.GetAllRockets().Add(rocket);
        }
    }
}