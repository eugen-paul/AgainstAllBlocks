using Godot;

public interface ICollisionBallPaddle
{
    Vector3 BallHitsPaddle(Ball ball, Paddle paddle, Vector3 normalCollisionToBAll);
}