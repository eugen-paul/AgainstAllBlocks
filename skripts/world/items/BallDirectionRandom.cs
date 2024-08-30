
using Godot;

public class BallDirectionRandom : ItemBehavior
{
    private readonly AbstractLevel level;

    public BallDirectionRandom(AbstractLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        foreach (var ball in level.GetAllBalls())
        {
            ball.Velocity = ball.Velocity.Rotated(Vector3.Up, GD.Randf() * Mathf.Pi * 2);
        }
    }
}