using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

public class UpgradeData
{
    [JsonConverter(typeof(JsonIListItemConverter<UpgradeType, JsonStringEnumConverter>))]
    public IList<UpgradeType> Slots { get; set; }

    public List<PurchasedUpgradesData> PurchasedUpgrades { get; set; } = new();

    public UpgradeData() { }

    public UpgradeData Copy()
    {
        var response = new UpgradeData
        {
            Slots = new List<UpgradeType>(Slots)
        };
        PurchasedUpgrades.ForEach(e => response.PurchasedUpgrades.Add(e.Copy()));
        return response;
    }

    public IDictionary<UpgradeType, PurchasedUpgradesData> PurchasedUpgradesAsMap()
    {
        return PurchasedUpgrades.ToImmutableDictionary(e => e.Type, e => e);
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
