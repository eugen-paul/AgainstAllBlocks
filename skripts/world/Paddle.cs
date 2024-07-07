using System;
using System.Diagnostics;
using Godot;

public partial class Paddle : CharacterBody3D
{
    [Export]
    public float MaxPaddleSpeed { get; set; } = 30.0f;

    public bool Pause { get; set; } = false;

    private float movePositionX = 0f;

    private readonly ICollisionBallPaddle collisionBallPaddle = new CollisionBallPaddle();

    public override void _PhysicsProcess(double delta)
    {
        var currentMove = MovePaddleToCursor(delta);

        var collision = MoveAndCollide(currentMove);
        if (collision != null && collision.GetCollider() is Node node)
        {
            if (node is Ball ball)
            {
                var normal = (ball.GlobalPosition - collision.GetPosition()).Normalized();
                var ballDirection = collisionBallPaddle.BallHitsPaddle(ball, this, normal);
                ball.Velocity = ballDirection * ball.Velocity.Length();
                ball.BallHitsPaddle();
            }
        }
    }

    private Vector3 MovePaddleToCursor(double delta)
    {
        var paddlePos3D = Position;
        var paddlePos2dX = GetViewport().GetCamera3D().UnprojectPosition(paddlePos3D).X;

        var viewportSizeX = GetViewport().GetVisibleRect().Size.X;
        // var mousePosX = GetViewport().GetMousePosition().X;
        var mousePosX = movePositionX;

        float sceeneSizeX = 22f;

        var paddleMoveX = mousePosX - paddlePos2dX;

        var d = paddleMoveX * 100f / viewportSizeX;
        var dx = d * sceeneSizeX / 100f;

        var maxXVelocity = MaxPaddleSpeed * (float)delta;

        if (dx > 0)
        {
            dx = Math.Min(dx, maxXVelocity);
        }
        else
        {
            dx = Math.Max(dx, -maxXVelocity);
        }

        return new Vector3(dx, 0, 0);
    }

    private static Vector3 RemoveY(Vector3 data)
    {
        return new Vector3(data.X, 0, data.Z);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            movePositionX = eventMouseMotion.Position.X;
        }
    }

    public float GetWidth()
    {
        var mesh = GetNode<CollisionShape3D>("CollisionShape3D");
        var box = (BoxShape3D)mesh.Shape;
        return box.Size.X;
    }
}
