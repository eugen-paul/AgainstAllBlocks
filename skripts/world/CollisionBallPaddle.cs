using System;
using Godot;

public class CollisionBallPaddle : ICollisionBallPaddle
{
    public Vector3 BallHitsPaddle(Ball ball, Paddle paddle, Vector3 normalCollisionToBall)
    {
        var paddleCenterX = paddle.GlobalPosition.X;
        var paddleWidth = paddle.GetWidth();

        var ballCenterX = ball.GlobalPosition.X;

        if (Math.Abs(normalCollisionToBall.Z) >= Math.Abs(normalCollisionToBall.X))
        {
            var degree = (paddleCenterX - ballCenterX) / (paddleWidth / 2f) * 60f;
            Vector3 direction;
            if (normalCollisionToBall.Z < 0)
            {
                direction = Vector3.Forward.Rotated(Vector3.Up, Mathf.DegToRad(degree));
            }
            else
            {
                direction = Vector3.Back.Rotated(Vector3.Up, Mathf.DegToRad(-degree));
            }
            return direction;
        }
        else
        {
            //Hit from left or from right.
            var rad = Mathf.DegToRad(60f);
            var zPositiv = ball.Velocity.Z > 0;
            Vector3 direction;
            if (paddleCenterX < ballCenterX)
            {
                //Ball hits paddle from right
                direction = Vector3.Right.Rotated(Vector3.Up, zPositiv ? -rad : rad);
            }
            else
            {
                //Ball hits paddle from left
                direction = Vector3.Left.Rotated(Vector3.Up, zPositiv ? rad : -rad);
            }
            return direction;
        }
    }

}