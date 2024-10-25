using Godot;

public enum GoldenBallType
{
    GRAY = 0,
    GOLD = 1,
}

[Tool]
public partial class GoldenBallBox : CenterContainer
{
    private GoldenBallType _ballType = GoldenBallType.GRAY;

    [Export]
    public GoldenBallType BallType
    {
        get => _ballType;
        set
        {
            _ballType = value;
            UpdateBallType();
        }
    }

    private void UpdateBallType()
    {
        GetNode<TextureRect>("Gold").Visible = _ballType == GoldenBallType.GOLD;
        GetNode<TextureRect>("Gray").Visible = _ballType == GoldenBallType.GRAY;
        GetNode<GpuParticles2D>("GPUParticles2DAnimation").Emitting = _ballType == GoldenBallType.GOLD;
    }

    public void PlayAnimation()
    {
        GetNode<GpuParticles2D>("GPUParticles2DAchievedAnimation").Emitting = true;
    }

    public void SetTooltipForBall(string text)
    {
        GD.Print("Set text to: "+ text);
        GetNode<TextureRect>("Gold").TooltipText = text;
        GetNode<TextureRect>("Gray").TooltipText = text;
    }
}
