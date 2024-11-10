

using System.Collections.Generic;
using System.Collections.Immutable;

public class EmptyUpgrade : AUpgrade
{
    private static readonly int MAX_LEVEL = 0;

    private static readonly string DESCRIPTION = "UPGRADE_EMPTY";

    private static readonly IList<string> LEVEL_DESCRIPTION = new List<string>()
    {
        "UPGRADE_EMPTY_0",
    }.ToImmutableList();

    private static readonly IList<string> TEXTURES = new List<string>()
    {
        "res://assets/textures/gui/upgrades/EMPTY-0.png",
    }.ToImmutableList();

    public EmptyUpgrade() : base(
        UpgradeType.EMPTY,
        0,
        MAX_LEVEL,
        DESCRIPTION,
        LEVEL_DESCRIPTION,
        TEXTURES
        )
    {
    }

    public override AUpgrade FromPurchasedUpgradesData(PurchasedUpgradesData data)
    {
        return new EmptyUpgrade
        {
            CurrentLevel = data.CurrentLevel
        };
    }

    public override AUpgrade Empty()
    {
        return new EmptyUpgrade();
    }
}