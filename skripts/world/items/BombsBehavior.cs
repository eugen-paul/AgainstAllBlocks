
using System;
using Godot;

public class BombsBehavior : ItemBehavior
{
    private readonly DefaultLevel level;

    public PackedScene BombScene { get; set; } = ResourceLoader.Load<PackedScene>(GameScenePaths.DEFAULT_BOMB_SCENE);

    public BombsBehavior(DefaultLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        var blocks = level.GetBlocks();
        Random random = new();
        int position = random.Next(0, blocks.Count);
        var blockFromArray = blocks[position];

        if (blockFromArray is ABlock block)
        {
            var bomb = BombScene.Instantiate<Bomb>();
            bomb.Level = level;
            bomb.SetTarget(block.GlobalPosition);
            level.AddChild(bomb);
            level.TemporaryAdd(bomb);
        }
    }
}