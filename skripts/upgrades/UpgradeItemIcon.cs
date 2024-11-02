using Godot;

public partial class UpgradeItemIcon : TextureRect
{
    public override Variant _GetDragData(Vector2 atPosition)
    {
        return new UpgradeItemDrag();
    }

    public void SetTexture(string path)
    {
        if (path != string.Empty)
        {
            Texture = GD.Load<Texture2D>(path);
        }
    }
}
