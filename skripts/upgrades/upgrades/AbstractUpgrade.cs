
using System.Collections.Generic;
using System.Collections.Immutable;

public abstract class AUpgrade
{
    public UpgradeType Type { private set; get; }

    public int CurrentLevel { protected set; get; }

    public int MaxLevel { private set; get; }

    public IList<string> Description { private set; get; }

    public IList<string> Textures { private set; get; }

    protected AUpgrade(UpgradeType Type, int CurrentLevel, int MaxLevel, IList<string> Description, IList<string> Textures)
    {
        this.Type = Type;
        this.CurrentLevel = CurrentLevel;
        this.MaxLevel = MaxLevel;
        this.Description = Description.ToImmutableList();
        this.Textures = Textures;
    }

    public abstract AUpgrade FromPurchasedUpgradesData(PurchasedUpgradesData data);

    public abstract AUpgrade Empty();
}