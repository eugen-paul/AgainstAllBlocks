using System;
using Godot;

public partial class Level13 : DefaultLevel
{
    protected override void InitBlockDestoyedCallbacks()
    {
        base.InitBlockDestoyedCallbacks();
        var bonusBlock = GetNode<Node3D>("GameButton/BgBlocks");
        var blocks = bonusBlock.FindChildren("Bonus*");
        foreach (var nodeBlock in blocks)
        {
            if (nodeBlock is ABlock block)
            {
                block.BlockDestroyed += BlockDestroid;
            }
        }
    }
}