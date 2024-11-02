using System;
using Godot;

public partial class UpgradeItemPanel : HBoxContainer
{
    public GodotObject LocalizationScriptObject { get; set; }

    public Action<AUpgrade> UpgradeAction { set; get; }

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
        GetNode<Label>("Label").Text = Var(_item.LevelDescription[Item.CurrentLevel]);
    }

    private string Var(string variableName)
    {
        return LocalizationScriptObject.Get(variableName).AsString();
    }

    private void OnUpgradeButtonPressed()
    {
        UpgradeAction?.Invoke(_item);
    }
}
