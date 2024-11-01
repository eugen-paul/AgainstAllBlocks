
using System.Collections.Generic;
using System.Collections.Immutable;
using Godot;

public enum UpgradeType
{
    EMPTY = 0,
    EXTRA_LIFE = 10,
    PADDLE_SPEED = 20,
}

public class UpgradeFactory
{
    private readonly IList<AUpgrade> AllUpgradesList = new List<AUpgrade>
    {
        new ExtraLifeUpgrade(),
        new PaddleSpeedUpgrade(),
    }
    .ToImmutableList();

    public IList<AUpgrade> GetListOfUpgrades()
    {
        var PurchasedUpgrades = GameComponets.Instance.Get<CurrentGame>().Game.Upgrades.PurchasedUpgradesAsMap();

        foreach (var item in PurchasedUpgrades)
        {
            GD.Print("loaded: item " + item.Key.ToString() + " level " + item.Value.CurrentLevel);
        }

        var response = new List<AUpgrade>();
        foreach (var item in AllUpgradesList)
        {
            if (PurchasedUpgrades.ContainsKey(item.Type))
            {
                response.Add(item.FromPurchasedUpgradesData(PurchasedUpgrades[item.Type]));
            }
            else
            {
                response.Add(item.Empty());
            }
        }

        return response.ToImmutableList();
    }
}
