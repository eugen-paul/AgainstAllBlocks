using Godot;

public partial class TooltipPanel : PanelContainer
{
    public void SetMyTooltipText(string text)
    {
        GetNode<Label>("Label").Text = text;
    }
}
