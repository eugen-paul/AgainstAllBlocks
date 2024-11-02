

using System.Collections.Generic;
using System.Collections.Immutable;

public class PaddleSpeedUpgrade : AUpgrade
{
    private static readonly int MAX_LEVEL = 3;

    private static readonly string DESCRIPTION = "UPGRADE_PADDLE_SPEED";
    private static readonly IList<string> LEVEL_DESCRIPTION = new List<string>()
    {
        "UPGRADE_PADDLE_SPEED_0",
        "UPGRADE_PADDLE_SPEED_1",
        "UPGRADE_PADDLE_SPEED_2",
        "UPGRADE_PADDLE_SPEED_3",
    }.ToImmutableList();

    private static readonly IList<string> TEXTURES = new List<string>()
    {
        "res://assets/textures/gui/upgrades/PADDLE_SPEED-0.png",
        "res://assets/textures/gui/upgrades/PADDLE_SPEED-1.png",
        "res://assets/textures/gui/upgrades/PADDLE_SPEED-2.png",
        "res://assets/textures/gui/upgrades/PADDLE_SPEED-3.png",
    }.ToImmutableList();

    public PaddleSpeedUpgrade() : base(
        UpgradeType.PADDLE_SPEED,
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
        return new PaddleSpeedUpgrade
        {
            CurrentLevel = data.CurrentLevel
        };
    }

    public override AUpgrade Empty()
    {
        return new PaddleSpeedUpgrade();
    }
}