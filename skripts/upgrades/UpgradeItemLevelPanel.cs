using System.Diagnostics;
using Godot;

public partial class UpgradeItemLevelPanel : PanelContainer, IUpgradeListener
{
    private static readonly string LEVEL_LABEL_PATH = "Panel/HBoxContainer/Panel/LevelLabel";
    private static readonly string TEXTURE_RECT_PATH = "Panel/HBoxContainer/TextureRect";
    private static readonly string TEXT_LABEL_PATH = "Panel/HBoxContainer/Panel3/TextLabel";
    private static readonly string COST_LABEL_PATH = "Panel/HBoxContainer/Panel2/HBoxContainer/CostLabel";
    private static readonly string CURRENT_BORDER_PATH = "CurrentBorder";
    private static readonly string OFF_PANEL_PATH = "OffPanel";

    public GodotObject LocalizationScriptObject { get; set; }

    public UpgradeType Type { get; private set; } = UpgradeType.EMPTY;
    public int Level { get; private set; } = 0;

    private bool init = false;

    public override void _Ready()
    {
        UpdateUi();
        UpdateData();
    }

    public override void _EnterTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().AddListener(this);
    }

    public override void _ExitTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().RemoveListener(this);
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

        var levelCost = UpgradeItemInfo.UpgradeItemInfos[Type].Cost[Level];
        GetNode<Label>(COST_LABEL_PATH).Text = levelCost.ToString();
    }

    public void UpgrageDataChange(AUpgradeSignal upgradeSignal)
    {
        if (upgradeSignal is UpgradeSignalUpdateUpgradeData signal && signal.ItemType == Type)
        {
            UpdateData();
        }
    }

    private void UpdateData()
    {
        var level = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentUpgradeLevel(Type);

        if (Level == level)
        {
            GetNode<Panel>(CURRENT_BORDER_PATH).Visible = true;
            GetNode<ColorRect>(OFF_PANEL_PATH).Visible = false;
        }
        else
        {
            GetNode<Panel>(CURRENT_BORDER_PATH).Visible = false;
            GetNode<ColorRect>(OFF_PANEL_PATH).Visible = true;
        }
    }

}
