using Godot;

public partial class UpgradeItemIcon : TextureRect
{
    public string texturePath = string.Empty;

    public UpgradeType Type { get; private set; }

    private int level = 0;

    public override Variant _GetDragData(Vector2 atPosition)
    {
        if (texturePath == string.Empty || level == 0)
        {
            return default;
        }

        var dragData = new UpgradeItemDrag();
        dragData.Init(Type);
        SetDragPreview(dragData.Preview);
        return dragData;
    }

    public void Init(UpgradeType type)
    {
        var level = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentUpgradeLevel(type);
        texturePath = UpgradeItemInfo.UpgradeItemInfos[type].Textures[level];
        Texture = GD.Load<Texture2D>(texturePath);

        Type = type;
        this.level = level;
    }
}
