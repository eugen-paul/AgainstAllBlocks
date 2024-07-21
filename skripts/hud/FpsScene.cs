using Godot;

public partial class FpsScene : CanvasLayer
{
    private FpsLabel _fpsLabel;

    public override void _Ready()
    {
        _fpsLabel = GetNode<FpsLabel>("FpsLabel");
        _fpsLabel.SetFps(0);
    }

    public override void _Process(double delta)
    {
        _fpsLabel.SetFps((int)Engine.GetFramesPerSecond());
    }
}
