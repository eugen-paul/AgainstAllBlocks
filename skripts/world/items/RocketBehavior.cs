using System;
using System.Collections.Generic;
using System.Linq;
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
        int rocketCount = 1;
        if (GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().IsUpgradeTypeActive(UpgradeType.ROCKET_COUNT))
        {
            rocketCount = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentUpgradeLevel(UpgradeType.ROCKET_COUNT) + 1;
        }

        var blocks = level.GetBlocks();
        var targets = GetRandomBlock(blocks, rocketCount);
        foreach (var b in targets)
        {
            var rocket = RocketScene.Instantiate<Rocket>();
            rocket.Level = level;
            rocket.Position = item.GlobalPosition;
            level.AddChild(rocket);
            rocket.SetTarget(b.GlobalPosition);
            level.TemporaryAdd(rocket);
        }
    }

    private static IList<ABlock> GetRandomBlock(Godot.Collections.Array<Node> blocks, int Count)
    {
        var random = new Random();
        var list = new List<ABlock>();
        foreach (var b in blocks)
        {
            if (b is ABlock block)
            {
                list.Add(block);
            }
        }

        var shuffled = list.OrderBy(x => random.Next()).ToList();

        return [.. shuffled.Take(Count)];
    }
}