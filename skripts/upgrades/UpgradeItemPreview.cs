using Godot;

public partial class UpgradeItemPreview : PanelContainer
{
    private static readonly string TEXTURE_RECT_PATH = "TextureRect";

    public void SetUpgrade(string texturePath)
    {
        GetNode<TextureRect>(TEXTURE_RECT_PATH).Texture = GD.Load<Texture2D>(texturePath);
    }
}
