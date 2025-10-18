using Godot;

public partial class DestroyBallOnEntry : Area3D
{
    public void OnArea3dBodyEntered(Node3D body)
    {
        if (body is not Ball ball)
        {
            return;
        }
        ball.BallLeavesScreen.Invoke(ball);
        ball.QueueFree();
    }
}
