using Godot;

public partial class GameSlotLevelLabel : Label
{
    public void SetLevel(int fps)
    {
        var text = Tr("Level: {0}");
        Text = string.Format(text, fps);
    }
}
