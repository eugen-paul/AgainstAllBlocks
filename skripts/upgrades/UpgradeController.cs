
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

public enum UpgradeType
{
    EMPTY = 0,
    EXTRA_LIFE = 10,
    PADDLE_SPEED = 20,
}

public interface IUpgradeListener
{
    void UpgrageDataChange(AUpgradeSignal upgradeSignal);
}

public class UpgradeController
{
    private Action<AUpgradeSignal> dataChangeAction;

    public void AddListener(IUpgradeListener l)
    {
        dataChangeAction += l.UpgrageDataChange;
    }

    public void RemoveListener(IUpgradeListener l)
    {
        dataChangeAction -= l.UpgrageDataChange;
    }

    private readonly IList<UpgradeType> UpgradesOrder = new List<UpgradeType>
    {
        UpgradeType.EXTRA_LIFE,
        UpgradeType.PADDLE_SPEED,
    }
    .ToImmutableList();

    public IList<Upgrade> GetCurrentUpgradesAsList()
    {
        var PurchasedUpgrades = GameComponets.Instance.Get<CurrentGame>().GetUpgradeData().PurchasedUpgradesAsMap();
        var response = new List<Upgrade>();
        foreach (var upgradeTyte in UpgradesOrder)
        {
            if (PurchasedUpgrades.ContainsKey(upgradeTyte))
            {
                response.Add(new Upgrade(PurchasedUpgrades[upgradeTyte]));
            }
            else
            {
                response.Add(new Upgrade(upgradeTyte));
            }
        }

        return response.ToImmutableList();
    }

    public int GetCurrentUpgradeLevel(UpgradeType type)
    {
        var PurchasedUpgrades = GameComponets.Instance.Get<CurrentGame>().GetUpgradeData().PurchasedUpgradesAsMap();
        if (PurchasedUpgrades.ContainsKey(type))
        {
            return PurchasedUpgrades[type].CurrentLevel;
        }

        return 0;
    }

    public IList<UpgradeType> GetCurrentSlots()
    {
        return GameComponets.Instance.Get<CurrentGame>().GetUpgradeData().Slots.ToImmutableList();
    }

    public void SetUpgradeInSlot(int slot, UpgradeType type)
    {
        
    }
}
