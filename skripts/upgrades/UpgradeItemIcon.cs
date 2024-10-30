using Godot;

public partial class UpgradeItemIcon : TextureRect
{
    public override Variant _GetDragData(Vector2 atPosition)
    {
        return new UpgradeItemDrag();
    }

}
