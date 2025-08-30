using Godot;

public partial class UpgradeGoldLabel : HBoxContainer, IUpgradeListener
{

    private static readonly string GOLD_LABEL_PATH = "Panel/MarginContainer/GoldValueLabel";

    public override void _EnterTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().AddListener(this);
        SetGoldValue(GameComponets.Instance.Get<CurrentGame>().GetGoldRest());
    }

    public override void _ExitTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().RemoveListener(this);
    }

    private void SetGoldValue(int Gold)
    {
        GetNode<Label>(GOLD_LABEL_PATH).Text = Gold.ToString();
    }

    public void UpgrageDataChange(AUpgradeSignal upgradeSignal)
    {
        if (upgradeSignal is UpgradeSignalUpdateGold s)
        {
            SetGoldValue(s.Gold);
        }
    }

}
