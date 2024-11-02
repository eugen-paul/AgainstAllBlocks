using Godot;

public partial class UpgradeItemLevelPanel : PanelContainer
{
    private static readonly string LEVEL_LABEL_PATH = "HBoxContainer/Panel/LevelLabel";
    private static readonly string TEXTURE_RECT_PATH = "HBoxContainer/UpgradeItemIcon";
    private static readonly string TEXT_LABEL_PATH = "HBoxContainer/TextLabel";

    public GodotObject LocalizationScriptObject { get; set; }

    public int Level { get; set; } = 0;
    public string TexturePath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LevelDescription { get; set; } = string.Empty;
    public bool Purchased { get; set; } = false;

    private bool ready = false;

    public override void _Ready()
    {
        ready = true;
        UpdateData();
    }

    private void UpdateData()
    {
        if (!ready)
        {
            return;
        }

        GetNode<Label>(LEVEL_LABEL_PATH).Text = Level.ToString();
        GetNode<UpgradeItemIcon>(TEXTURE_RECT_PATH).SetTexture(TexturePath);
        if (LevelDescription != string.Empty)
        {
            GetNode<Label>(TEXT_LABEL_PATH).Text = LocalizationScriptObject.Get(LevelDescription).AsString();
        }
    }
}
