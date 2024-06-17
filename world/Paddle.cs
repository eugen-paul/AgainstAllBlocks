using System;
using Godot;

public partial class Paddle : CharacterBody3D
{
    [Export]
    public float MaxPaddleSpeed { get; set; } = 30.0f;

    public bool Pause { get; set; } = false;

    private float movePositionX = 0f;

    public override void _PhysicsProcess(double delta)
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

        var collision = MoveAndCollide(new Vector3(dx, 0, 0));
        if (collision != null && collision.GetCollider() is Node node)
        {
            if (node is Ball ball)
            {
                ball.Velocity += new Vector3(dx, 0, 0);
            }
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            movePositionX = eventMouseMotion.Position.X;
        }
    }
}
