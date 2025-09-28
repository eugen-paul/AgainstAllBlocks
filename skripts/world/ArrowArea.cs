using System.Diagnostics;
using Godot;

public partial class ArrowArea : Node3D
{
    [Export]
    private float FromDegree { get; set; } = -10f;

    [Export]
    private float ToDegree { get; set; } = 10f;

    public void OnArea3dBodyEntered(Node3D body)
    {
        if (body is Ball ball)
        {
            var ballVelocity = ball.Velocity.Length();
            var randomDegree = GD.RandRange(FromDegree, ToDegree);
            var newDirection = Vector3.Back.Rotated(Vector3.Up, GlobalRotation.Y).Normalized();
            newDirection = newDirection.Rotated(Vector3.Up, (float)Mathf.DegToRad(randomDegree)).Normalized();
            ball.Velocity = newDirection * ballVelocity;
        }
    }

}
