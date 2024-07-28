using Godot;

public partial class LevelBall : TextureRect
{

    private const string GOLD_BALL_PATH = "res://assets/textures/gui/ball_golden.png";

    private bool goldBall = false;

    public void SetGoldBall()
    {
        Texture = GD.Load<Texture2D>(GOLD_BALL_PATH);
    }
}
