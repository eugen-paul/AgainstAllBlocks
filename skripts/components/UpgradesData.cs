using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

public class UpgradeData
{
    public UpgradeSlotData Slots { get; set; } = new();
    public List<PurchasedUpgradesData> PurchasedUpgrades { get; set; } = new();

    public UpgradeData() { }

    public UpgradeData Copy()
    {
        var response = new UpgradeData();
        PurchasedUpgrades.ForEach(e => response.PurchasedUpgrades.Add(e.Copy()));
        return response;
    }

    public IDictionary<UpgradeType, PurchasedUpgradesData> PurchasedUpgradesAsMap()
    {
        return PurchasedUpgrades.ToImmutableDictionary(e => e.Type, e => e);
    }
}

public class UpgradeSlotData
{
    public int SlotsCount { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UpgradeType Slot1 { get; set; } = UpgradeType.EMPTY;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UpgradeType Slot2 { get; set; } = UpgradeType.EMPTY;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UpgradeType Slot3 { get; set; } = UpgradeType.EMPTY;

    public UpgradeSlotData() { }

    public UpgradeSlotData Copy()
    {
        return new UpgradeSlotData
        {
            SlotsCount = SlotsCount,
            Slot1 = Slot1,
            Slot2 = Slot2,
            Slot3 = Slot3
        };
    }
}

public class PurchasedUpgradesData
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UpgradeType Type { get; set; }
    public int CurrentLevel { get; set; } = 0;

    public PurchasedUpgradesData() { }

    public PurchasedUpgradesData Copy()
    {
        return new PurchasedUpgradesData
        {
            CurrentLevel = CurrentLevel,
            Type = Type,
        };
    }
}
