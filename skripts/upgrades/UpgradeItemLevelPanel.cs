using Godot;

public partial class UpgradeItemLevelPanel : PanelContainer
{
    private static readonly string LEVEL_LABEL_PATH = "HBoxContainer/Panel/LevelLabel";
    private static readonly string TEXTURE_RECT_PATH = "HBoxContainer/TextureRect";
    private static readonly string TEXT_LABEL_PATH = "HBoxContainer/TextLabel";

    public GodotObject LocalizationScriptObject { get; set; }

    public UpgradeType Type { get; private set; } = UpgradeType.EMPTY;
    public int Level { get; private set; } = 0;

    private bool init = false;

    public override void _Ready()
    {
        UpdateUi();
    }

    public void Init(UpgradeType type, int level)
    {
        init = true;
        Type = type;
        Level = level;

        var texturePath = UpgradeItemInfo.UpgradeItemInfos[Type].Textures[level];
        GetNode<TextureRect>(TEXTURE_RECT_PATH).Texture = GD.Load<Texture2D>(texturePath);
        GetNode<Label>(LEVEL_LABEL_PATH).Text = Level.ToString();
    }

    private void UpdateUi()
    {
        if (!init)
        {
            return;
        }

        var levelDescription = UpgradeItemInfo.UpgradeItemInfos[Type].LevelDescription[Level];
        GetNode<Label>(TEXT_LABEL_PATH).Text = LocalizationScriptObject.Get(levelDescription).AsString();
    }
}
