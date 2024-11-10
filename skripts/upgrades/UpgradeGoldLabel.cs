using Godot;

public partial class UpgradeGoldLabel : HBoxContainer, IUpgradeListener
{
    public override void _EnterTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().AddListener(this);
        SetGoldValue(GameComponets.Instance.Get<CurrentGame>().GetGoldRest());
    }

    public override void _ExitTree()
    {
        GameComponets.Instance.Get<CurrentGame>().GetUpgradeController().AddListener(this);
    }

    private void SetGoldValue(int Gold)
    {
        GetNode<Label>("GoldValueLabel").Text = Gold.ToString();
    }

    public void UpgrageDataChange(AUpgradeSignal upgradeSignal)
    {
        if (upgradeSignal is UpgradeSignalUpdateGold s)
        {
            SetGoldValue(s.Gold);
        }
    }

}
