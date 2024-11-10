using Godot;

public partial class UpgradeSlot : PanelContainer
{
    private static readonly string TEXTURE_RECT_ITEM_PATH = "TextureRectItem";
    private static readonly string TEXTURE_RECT_FOREGROUND_PATH = "TextureRectForeground";
    private static readonly string TEXTURE_ITEM_EMPTY = "res://assets/textures/gui/UpgradeSlotBG-EMPTY.png";
    private static readonly string TEXTURE_ITEM_SET = "res://assets/textures/gui/UpgradeSlotBG-SET.png";

    public override void _Ready()
    {
        SetForegroundEmpty();
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (data.VariantType != Variant.Type.Object && data.AsGodotObject() is not UpgradeItemDrag)
        {
            return false;
        }
        var item = data.AsGodotObject() as UpgradeItemDrag;
        return true;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var item = data.AsGodotObject() as UpgradeItemDrag;
        GD.Print("Catch: " + item.GetType());
    }

    public void SetItem(UpgradeType itemType)
    {
        if (itemType == UpgradeType.EMPTY)
        {
            SetForegroundEmpty();
        }
        else
        {
            SetForegroundNotEmpty();
        }
        var purchasedUpgrades = GameComponets.Instance.Get<CurrentGame>().GetUpgradeData().PurchasedUpgradesAsMap();
        var currentLevel = 0;
        if (purchasedUpgrades.ContainsKey(itemType))
        {
            currentLevel = purchasedUpgrades[itemType].CurrentLevel;
        }

        SetItemTexture(UpgradeItemInfo.UpgradeItemInfos[itemType].Textures[currentLevel]);
    }

    private void SetForegroundEmpty()
    {
        GetNode<TextureRect>(TEXTURE_RECT_FOREGROUND_PATH).Texture = GD.Load<Texture2D>(TEXTURE_ITEM_EMPTY);
    }

    private void SetForegroundNotEmpty()
    {
        GetNode<TextureRect>(TEXTURE_RECT_FOREGROUND_PATH).Texture = GD.Load<Texture2D>(TEXTURE_ITEM_SET);
    }

    private void SetItemTexture(string path)
    {
        GetNode<TextureRect>(TEXTURE_RECT_ITEM_PATH).Texture = GD.Load<Texture2D>(path);
    }
}
