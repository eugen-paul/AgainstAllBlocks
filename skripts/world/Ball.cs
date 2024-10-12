using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

public enum BallSounds
{
    KICK,
    HIT_BALL,
    HIT_BLOCK,
    HIT_PADDLE,
    HIT_WALL,
}

public partial class Ball : CharacterBody3D
{
    public Action<Ball> BallLeavesScreen;

    public float StartSpeed { get; set; } = 23.0f;

    public float MaxSpeed { get; set; } = 40f;

    public float MinSpeed { get; set; } = 15f;

    public float Weight { get; set; } = 1.0f;

    [Export]
    public int MaxScoreBomus { get; set; } = 5;

    private int _scoreBonus = 0;

    [Export]
    public int ScoreBonus
    {
        get => _scoreBonus;
        set
        {
            _scoreBonus = Math.Min(value, MaxScoreBomus);
        }
    }

    private readonly static IDictionary<BallSounds, string> SOUND_TO_PATH = new Dictionary<BallSounds, string> {
            {  BallSounds.KICK, "Kick" },
            {  BallSounds.HIT_BALL, "HitBall" },
            {  BallSounds.HIT_BLOCK, "HitBlock" },
            {  BallSounds.HIT_PADDLE, "HitPaddle" },
            {  BallSounds.HIT_WALL, "HitWall" },
        }
        .ToImmutableDictionary();

    private readonly ICollisionBallPaddle collisionBallPaddle = new CollisionBallPaddle();

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

                LimitSpeed(ball);
                LimitSpeed(this);
                PlaySound(BallSounds.HIT_BALL);
            }
            else if (node.IsInGroup("Wall"))
            {
                Velocity = Velocity.Bounce(collision.GetNormal());
                Velocity = RemoveY(Velocity);
                PlaySound(BallSounds.HIT_WALL);
            }
            else if (node.IsInGroup("Block"))
            {
                if (node is Hitable block)
                {
                    block.Hit(ScoreBonus);
                }
                if (node is ABlock)
                {
                    BallHitsBlock();
                }
                Velocity = Velocity.Bounce(collision.GetNormal());
                Velocity = RemoveY(Velocity);
                PlaySound(BallSounds.HIT_BLOCK);
            }
            else if (node.IsInGroup("Paddle"))
            {
                if (node is Paddle paddle)
                {
                    var direction = collisionBallPaddle.BallHitsPaddle(this, paddle, collision.GetNormal());
                    Velocity = direction * Velocity.Length();
                    Velocity = RemoveY(Velocity);
                    BallHitsPaddle();
                    PlaySound(BallSounds.HIT_PADDLE);
                }
            }
            else
            {
                Debug.Print("Collision with somtthing. " + node.ToString());
            }
        }
    }

    private static void LimitSpeed(Ball ball)
    {
        if (ball.Velocity.Length() < ball.MinSpeed)
        {
            ball.Velocity = ball.Velocity.Normalized() * ball.MinSpeed;
        }
        if (ball.Velocity.Length() > ball.MaxSpeed)
        {
            ball.Velocity = ball.Velocity.Normalized() * ball.MaxSpeed;
        }
    }

    private static Vector3 RemoveY(Vector3 data)
    {
        return new Vector3(data.X, 0, data.Z);
    }

    private void OnVisibilityNotifierScreenExited()
    {
        BallLeavesScreen.Invoke(this);
        QueueFree();
    }

    private void BallHitsBlock()
    {
        ScoreBonus++;
    }

    public void BallHitsPaddle()
    {
        ScoreBonus = 0;
    }

    public void PlaySound(BallSounds sound)
    {
        GetNode<AudioStreamPlayer>(SOUND_TO_PATH[sound]).Play();
    }
}
