

using System.Collections.Generic;
using System.Collections.Immutable;

public class ExtraLifeUpgrade : AUpgrade
{
    private static readonly int MAX_LEVEL = 3;

    private static readonly string DESCRIPTION = "UPGRADE_EXTRA_LIFE";

    private static readonly IList<string> LEVEL_DESCRIPTION = new List<string>()
    {
        "UPGRADE_EXTRA_LIFE_0",
        "UPGRADE_EXTRA_LIFE_1",
        "UPGRADE_EXTRA_LIFE_2",
        "UPGRADE_EXTRA_LIFE_3",
    }.ToImmutableList();

    private static readonly IList<string> TEXTURES = new List<string>()
    {
        "res://assets/textures/gui/upgrades/EXTRA_LIFE-0.png",
        "res://assets/textures/gui/upgrades/EXTRA_LIFE-1.png",
        "res://assets/textures/gui/upgrades/EXTRA_LIFE-2.png",
        "res://assets/textures/gui/upgrades/EXTRA_LIFE-3.png",
    }.ToImmutableList();

    public ExtraLifeUpgrade() : base(
        UpgradeType.EXTRA_LIFE,
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
        return new ExtraLifeUpgrade
        {
            CurrentLevel = data.CurrentLevel
        };
    }

    public override AUpgrade Empty()
    {
        return new ExtraLifeUpgrade();
    }
}