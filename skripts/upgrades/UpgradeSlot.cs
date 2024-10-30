using Godot;

public partial class UpgradeSlot : PanelContainer
{
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (data.VariantType != Variant.Type.Object && data.AsGodotObject() is not UpgradeItemDrag)
        {
            return false;
        }
        var item = data.AsGodotObject() as UpgradeItemDrag;
        return false;
    }
}
