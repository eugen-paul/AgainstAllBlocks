using Godot;

public partial class Portal : Node3D
{
    private void OnPortalIn(Node3D body)
    {
        if (body is Ball ball)
        {
            ball.Velocity = Vector3.Zero;
            ball.GlobalPosition = new Vector3(0, -1000, 0);
            GetNode<AudioStreamPlayer>("IN/AudioStreamPlayer").Play();

            GetTree().CreateTimer(1.5, false, true).Timeout += () => DoPortalOut(ball);
        }
    }

    private void DoPortalOut(Ball ball)
    {
        ball.GlobalPosition = GlobalPosition + new Vector3(0, 2, 0);
        var OUT = GetNode<Node3D>("OUT");
        ball.Velocity = Vector3.Forward.Rotated(Vector3.Up, OUT.GlobalRotation.Y) * ball.StartSpeed;

        var outPoint = GetNode<Node3D>("OUT/OutPoint").GlobalPosition;
        ball.GlobalPosition = new Vector3(outPoint.X, 0.5f, outPoint.Z);

        GetNode<AudioStreamPlayer>("OUT/AudioStreamPlayer").Play();
    }
}
