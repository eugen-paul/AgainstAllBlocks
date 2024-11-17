using System;
using Godot;

public partial class UpgradeSlot : PanelContainer, IUpgradeListener
{
    private static readonly string TEXTURE_RECT_ITEM_PATH = "TextureRectItem";
    private static readonly string TEXTURE_RECT_FOREGROUND_PATH = "TextureRectForeground";
    private static readonly string TEXTURE_ITEM_EMPTY = "res://assets/textures/gui/UpgradeSlotBG-EMPTY.png";
    private static readonly string TEXTURE_ITEM_SET = "res://assets/textures/gui/UpgradeSlotBG-SET.png";

    private int SlotNr { get; set; } = -1;

    private UpgradeType ItemType { get; set; }

    public override void _Ready()
    {
        SetForeground(TEXTURE_ITEM_EMPTY);
        UpdateItem();
    }

    public override void _EnterTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().AddListener(this);
    }

    public override void _ExitTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().RemoveListener(this);
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (data.VariantType != Variant.Type.Object && data.AsGodotObject() is not UpgradeItemDrag)
        {
            return false;
        }
        return true;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        if (data.AsGodotObject() is UpgradeItemDrag item)
        {
            GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().SetUpgradeInSlot(SlotNr, item.Type);
        }
    }

    public void Init(int slotNr, UpgradeType itemType)
    {
        SlotNr = slotNr;
        ItemType = itemType;
    }

    private void UpdateItem()
    {
        if (ItemType == UpgradeType.EMPTY)
        {
            SetForeground(TEXTURE_ITEM_EMPTY);
        }
        else
        {
            SetForeground(TEXTURE_ITEM_SET);
        }
        var currentLevel = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentUpgradeLevel(ItemType);
        SetItemTexture(UpgradeItemInfo.UpgradeItemInfos[ItemType].Textures[currentLevel]);
    }

    private void SetForeground(string path)
    {
        GetNode<TextureRect>(TEXTURE_RECT_FOREGROUND_PATH).Texture = GD.Load<Texture2D>(path);
    }

    private void SetItemTexture(string path)
    {
        GetNode<TextureRect>(TEXTURE_RECT_ITEM_PATH).Texture = GD.Load<Texture2D>(path);
    }

    public void UpgrageDataChange(AUpgradeSignal upgradeSignal)
    {
        switch (upgradeSignal)
        {
            case UpgradeSignalUpdateSlot signal:
                if (signal.SlotNr == SlotNr)
                {
                    ItemType = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentSlots()[SlotNr];
                    UpdateItem();
                }
                break;
            case UpgradeSignalUpdateUpgradeData signal2:
                if (signal2.ItemType == ItemType)
                {
                    UpdateItem();
                }
                break;
        }
    }

}
