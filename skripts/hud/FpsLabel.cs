using Godot;

public partial class FpsLabel : Label
{
    public void SetFps(int fps)
    {
        Text = fps.ToString();
    }
}
