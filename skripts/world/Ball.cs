using Godot;
using System;
using System.Diagnostics;

public partial class Ball : CharacterBody3D
{
    [Signal]
    public delegate void BallLeavesScreenEventHandler();

    public float StartSpeed { get; set; } = 20.0f;

    public float Weight { get; set; } = 1.0f;

    public int ScoreBonus { get; set; } = 0;

    private ICollisionBallPaddle collisionBallPaddle = new CollisionBallPaddle();

    public override void _PhysicsProcess(double delta)
    {
        var collision = MoveAndCollide(Velocity * (float)delta);
        if (collision != null && collision.GetCollider() is Node node)
        {
            if (node is Ball ball)
            {
                //Normalvektor (Richtungsvektor zwischen den Zentren der Bälle):
                var normal = (GlobalTransform.Origin - ball.GlobalTransform.Origin).Normalized();

                //Geschwindigkeiten entlang des Normalvektors:
                var v1n = normal.Dot(Velocity);
                var v2n = normal.Dot(ball.Velocity);

                var m1 = Weight;
                var m2 = ball.Weight;

                //Neue Geschwindigkeiten entlang des Normalvektors nach der Kollision:
                var v1n_new = (v1n * (m1 - m2) + 2 * m2 * v2n) / (m1 + m2);
                var v2n_new = (v2n * (m2 - m1) + 2 * m1 * v1n) / (m1 + m2);

                //Berechne die neuen Geschwindigkeitsvektoren:
                var v1n_new_vec = v1n_new * normal;
                var v2n_new_vec = v2n_new * normal;
                var v1t_vec = Velocity - v1n * normal;
                var v2t_vec = ball.Velocity - v2n * normal;

                Velocity = v1n_new_vec + v1t_vec;
                Velocity = RemoveY(Velocity);

                ball.Velocity = v2n_new_vec + v2t_vec;
                ball.Velocity = RemoveY(ball.Velocity);
            }
            else if (node.IsInGroup("Wall"))
            {
                Velocity = Velocity.Bounce(collision.GetNormal());
                Velocity = RemoveY(Velocity);
            }
            else if (node.IsInGroup("Block"))
            {
                if (node is Block block)
                {
                    block.Hit(ScoreBonus);
                    ballHitsBlock();
                }
                Velocity = Velocity.Bounce(collision.GetNormal());
                Velocity = RemoveY(Velocity);
            }
            else if (node.IsInGroup("Paddle"))
            {
                if (node is Paddle paddle)
                {
                    Debug.Print("Ball hits paddle.");
                    var direction = collisionBallPaddle.BallHitsPaddle(this, paddle, collision.GetNormal());
                    Velocity = direction * Velocity.Length();
                    Velocity = RemoveY(Velocity);
                    ballHitsPaddle();
                }
            }
            else
            {
                Debug.Print("Collision with somtthing. " + node.ToString());
            }
        }
    }

    private static Vector3 RemoveY(Vector3 data)
    {
        return new Vector3(data.X, 0, data.Z);
    }

    private void OnVisibilityNotifierScreenExited()
    {
        EmitSignal(SignalName.BallLeavesScreen);
        QueueFree();
    }

    public void ballHitsBlock()
    {
        ScoreBonus++;
    }

    public void ballHitsPaddle()
    {
        ScoreBonus = 0;
    }
}
