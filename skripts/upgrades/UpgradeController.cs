
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
    /// <summary>
    /// The function is called by the upgrade controller every time a change has occurred in the upgrades.
    /// </summary>
    /// <param name="upgradeSignal"></param>
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
        UpgradeType.EMPTY,
        UpgradeType.EXTRA_LIFE,
        UpgradeType.PADDLE_SPEED,
    }
    .ToImmutableList();

    public IList<UpgradeType> GetUpgradesOrder() => UpgradesOrder;

    public bool IsUpgradeTypeActive(UpgradeType type)
    {
        foreach (var slot in Upgrades().Slots)
        {
            if (slot == type)
            {
                return true;
            }
        }
        return false;
    }

    public int GetCurrentUpgradeLevel(UpgradeType type)
    {
        var PurchasedUpgrades = Upgrades().PurchasedUpgradesAsMap();
        if (PurchasedUpgrades.TryGetValue(type, out PurchasedUpgradesData value))
        {
            return value.CurrentLevel;
        }

        return 0;
    }

    public IList<UpgradeType> GetCurrentSlots()
    {
        return Upgrades().Slots.ToImmutableList();
    }

    public void SetUpgradeInSlot(int slotNr, UpgradeType type)
    {
        var slots = Upgrades().Slots;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == type)
            {
                slots[i] = UpgradeType.EMPTY;
                SendSlotChange(i);
            }
        }

        Upgrades().Slots[slotNr] = type;
        SendSlotChange(slotNr);

        GameComponets.Instance.Get<CurrentGame>().Save();
    }

    public void BuyNewUpgradeSlot()
    {
        var goldRest = GameComponets.Instance.Get<CurrentGame>().GetGoldRest();
        var nextSlotNr = GetCurrentSlots().Count;
        var nextSlotCost = UpgradeItemInfo.SlotsCost[nextSlotNr];
        if (nextSlotCost <= goldRest)
        {
            Upgrades().Slots.Add(UpgradeType.EMPTY);
            SendSlotCountChange();
            SendGoldChange();
            GameComponets.Instance.Get<CurrentGame>().Save();
        }
    }

    public void BuyNewUpgrade(UpgradeType type)
    {
        var nextLevel = GetCurrentUpgradeLevel(type) + 1;
        if (nextLevel > UpgradeItemInfo.UpgradeItemInfos[type].MaxLevel)
        {
            return;
        }

        var goldRest = GameComponets.Instance.Get<CurrentGame>().GetGoldRest();
        var nextLevelCost = UpgradeItemInfo.UpgradeItemInfos[type].Cost[nextLevel];
        if (nextLevelCost <= goldRest)
        {
            PurchasedUpgradesData data = Upgrades().PurchasedUpgrades.Find(e => e.Type == type);
            if (data == null)
            {
                data = new PurchasedUpgradesData
                {
                    Type = type
                };
                Upgrades().PurchasedUpgrades.Add(data);
            }
            data.CurrentLevel = nextLevel;

            SendUpgradeChange(type);
            SendGoldChange();

            GameComponets.Instance.Get<CurrentGame>().Save();
        }
    }

    private void SendSlotChange(int slotNr)
    {
        dataChangeAction(new UpgradeSignalUpdateSlot(slotNr));
    }

    private void SendSlotCountChange()
    {
        dataChangeAction(new UpgradeSignalUpdateSlotsCount());
    }

    private void SendUpgradeChange(UpgradeType type)
    {
        dataChangeAction(new UpgradeSignalUpdateUpgradeData(type));
    }

    private void SendGoldChange()
    {
        dataChangeAction(new UpgradeSignalUpdateGold(GameComponets.Instance.Get<CurrentGame>().GetGoldRest()));
    }

    private static UpgradeData Upgrades() => GameComponets.Instance.Get<CurrentGame>().GetUpgradeData();
}
