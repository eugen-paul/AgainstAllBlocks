using Godot;

public partial class FpsScene : CanvasLayer
{
    private FpsLabel fpsLabel;

    public override void _Ready()
    {
        fpsLabel = GetNode<FpsLabel>("FpsLabel");
        fpsLabel.SetFps(0);
    }

    public override void _Process(double delta)
    {
        fpsLabel.SetFps((int)Engine.GetFramesPerSecond());
    }
}
