using Godot;

public partial class Footballer : Path3D
{
    private const string PATHFOLLOW3D_CHILD_NAME = "PathFollow3D";
    private const string Sprite3D_CHILD_NAME = "PathFollow3D/Footballer/Sprite3D";

    [Export]
    private float Speed { get; set; } = 1.0f;

    [Export]
    private float FromDegree { get; set; } = -10f;
    
    [Export]
    private float ToDegree { get; set; } = 10f;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        MoveGoalieOnPath(delta);
    }

    private void MoveGoalieOnPath(double delta)
    {
        if (GetNodeOrNull<PathFollow3D>(PATHFOLLOW3D_CHILD_NAME) is PathFollow3D pathFollow)
        {
            pathFollow.Progress += (float)delta * Speed;

            if (GetNodeOrNull<Sprite3D>(Sprite3D_CHILD_NAME) is Sprite3D sprite)
            {
                sprite.FlipH = pathFollow.ProgressRatio > .5f;
            }
        }
    }

    public void OnArea3dBodyEntered(Node3D body)
    {
        if (body is Ball ball)
        {
            var ballVelocity = ball.Velocity.Length();
            var randomDegree = GD.RandRange(FromDegree, ToDegree);
            var newDirection = Vector3.Back.Rotated(Vector3.Up, (float)Mathf.DegToRad(randomDegree)).Normalized();
            ball.Velocity = newDirection * ballVelocity;

            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        }
    }
}
