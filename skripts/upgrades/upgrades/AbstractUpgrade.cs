
using System.Collections.Generic;
using System.Collections.Immutable;

public abstract class AUpgrade
{
    public UpgradeType Type { private set; get; }

    public int CurrentLevel { protected set; get; }

    public int MaxLevel { private set; get; }

    public string Description {private set; get;}

    public IList<string> LevelDescription { private set; get; }

    public IList<string> Textures { private set; get; }

    protected AUpgrade(UpgradeType Type, int CurrentLevel, int MaxLevel, string Description, IList<string> LevelDescription, IList<string> Textures)
    {
        this.Type = Type;
        this.CurrentLevel = CurrentLevel;
        this.MaxLevel = MaxLevel;
        this.Description = Description;
        this.LevelDescription = LevelDescription.ToImmutableList();
        this.Textures = Textures;
    }

    public abstract AUpgrade FromPurchasedUpgradesData(PurchasedUpgradesData data);

    public abstract AUpgrade Empty();
}