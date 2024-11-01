using Godot;

public partial class UpgradeItemPanel : HBoxContainer
{
    private GodotObject scriptObject;

    public AUpgrade _item;
    public AUpgrade Item
    {
        get => _item;
        set
        {
            _item = value;
            UpdateData();
        }
    }

    private bool ready = false;

    public override void _Ready()
    {
        GDScript script = GD.Load<GDScript>("res://scenes/localization/Localization_Upgrades.gd");
        scriptObject = (GodotObject)script.New();

        ready = true;
        UpdateData();
    }

    private void UpdateData()
    {
        if (!ready || _item == null)
        {
            return;
        }
        GetNode<UpgradeItemIcon>("TextureRect").SetTexture(_item.Textures[Item.CurrentLevel]);
        GetNode<Label>("Label").Text = Var(_item.Description[Item.CurrentLevel]);
    }

    private string Var(string variableName)
    {
        return scriptObject.Get(variableName).AsString();
    }
}
