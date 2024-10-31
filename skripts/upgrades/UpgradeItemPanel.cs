using Godot;

public partial class UpgradeItemPanel : HBoxContainer
{
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
        GetNode<Label>("Label").Text = _item.Description[Item.CurrentLevel];
    }
}
