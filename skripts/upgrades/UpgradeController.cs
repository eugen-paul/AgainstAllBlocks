
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public enum UpgradeType
{
    EMPTY = 0,
    EXTRA_LIFE = 10,
    PADDLE_SPEED = 20,
}

public interface IUpgradeListener
{
    void UpgrageDataChange();
}

public class UpgradeController
{
    private Action dataChangeAction;

    public void AddListener(IUpgradeListener l)
    {
        dataChangeAction += l.UpgrageDataChange;
    }

    public void RemoveListener(IUpgradeListener l)
    {
        dataChangeAction -= l.UpgrageDataChange;
    }

    private readonly IList<AUpgrade> AllUpgradesList = new List<AUpgrade>
    {
        new ExtraLifeUpgrade(),
        new PaddleSpeedUpgrade(),
    }
    .ToImmutableList();

    public IList<AUpgrade> GetCurrentListOfUpgrades()
    {
        var PurchasedUpgrades = GameComponets.Instance.Get<CurrentGame>().GetUpgradeData().PurchasedUpgradesAsMap();

        // foreach (var item in PurchasedUpgrades)
        // {
        //     GD.Print("loaded: item " + item.Key.ToString() + " level " + item.Value.CurrentLevel);
        // }

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

    public IList<UpgradeType> GetCurrentSlots()
    {
        return GameComponets.Instance.Get<CurrentGame>().GetUpgradeData().Slots.ToList();
    }
}
