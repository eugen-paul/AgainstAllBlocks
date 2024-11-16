using System;
using Godot;

public partial class UpgradeItemPanel : HBoxContainer
{
    public GodotObject LocalizationScriptObject { get; set; }

    public Action<UpgradeType> UpgradeAction { set; get; }

    public UpgradeType Type { get; private set; }

    private bool init = false;

    public override void _Ready()
    {
        UpdateData();
    }

    public void Init(UpgradeType type)
    {
        Type = type;
        init = true;
    }

    private void UpdateData()
    {
        if (!init)
        {
            return;
        }

        var level = GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().GetCurrentUpgradeLevel(Type);
        var levelDescription = UpgradeItemInfo.UpgradeItemInfos[Type].LevelDescription[level];

        GetNode<UpgradeItemIcon>("TextureRect").Init(Type);
        GetNode<Label>("Label").Text = Var(levelDescription);
    }

    private string Var(string variableName)
    {
        return LocalizationScriptObject.Get(variableName).AsString();
    }

    private void OnUpgradeButtonPressed()
    {
        UpgradeAction?.Invoke(Type);
    }
}
