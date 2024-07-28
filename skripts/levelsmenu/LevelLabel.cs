using Godot;

public partial class LevelLabel : Label
{
    public void SetLevel(int level)
    {
        Text = level.ToString();
    }
}
