

using System.Collections.Generic;
using System.Collections.Immutable;

public class ExtraLifeUpgrade : AUpgrade
{
    private static readonly int MAX_LEVEL = 3;

    private static readonly IList<string> EXTRA_LIFE_DESCRIPTION = new List<string>()
    {
        "0 extra Life",
        "1 extra Life",
        "2 extra Lifes",
        "3 extra Lifes",
    }.ToImmutableList();

    private static readonly IList<string> EXTRA_LIFE_TEXTURES = new List<string>()
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
        EXTRA_LIFE_DESCRIPTION,
        EXTRA_LIFE_TEXTURES
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