
using System.Collections.Generic;

public class Upgrade
{
    public UpgradeType Type { private set; get; }

    public int CurrentLevel { protected set; get; } = 0;

    public int MaxLevel
    {
        get
        {
            return UpgradeItemInfo.UpgradeItemInfos[Type].MaxLevel;
        }
    }

    public string Description
    {
        get
        {
            return UpgradeItemInfo.UpgradeItemInfos[Type].Description;
        }
    }

    public IList<string> LevelDescription
    {
        get
        {
            return UpgradeItemInfo.UpgradeItemInfos[Type].LevelDescription;
        }
    }

    public IList<int> Cost
    {
        get
        {
            return UpgradeItemInfo.UpgradeItemInfos[Type].Cost;
        }
    }

    public IList<string> Textures
    {
        get
        {
            return UpgradeItemInfo.UpgradeItemInfos[Type].Textures;
        }
    }

    public Upgrade(UpgradeType Type, int CurrentLevel)
    {
        this.Type = Type;
        this.CurrentLevel = CurrentLevel;
    }

    public Upgrade(PurchasedUpgradesData data) : this(data.Type, data.CurrentLevel) { }

    public Upgrade(UpgradeType Type) : this(Type, 0) { }
}