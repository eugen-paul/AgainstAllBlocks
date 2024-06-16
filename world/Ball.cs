using Godot;
using System.Diagnostics;

public partial class Ball : CharacterBody3D
{
    [Signal]
    public delegate void BallLeavesScreenEventHandler();

    public float Speed { get; set; } = 20.0f;

    public float Weight { get; set; } = 1.0f;

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
                Velocity = removeY(Velocity);

                ball.Velocity = v2n_new_vec + v2t_vec;
                ball.Velocity = removeY(ball.Velocity);
            }
            else if (node.IsInGroup("Wall") || node.IsInGroup("Block"))
            {
                Velocity = Velocity.Bounce(collision.GetNormal());
                Velocity = removeY(Velocity);
            }
            else if (node.IsInGroup("Paddle"))
            {
                Velocity = Velocity.Bounce(collision.GetNormal());
                Velocity += new Vector3(GD.Randf() * 0.2f - 0.1f, 0, 0); //TODO
                Velocity = removeY(Velocity);
            }
            else
            {
                Debug.Print("Collision with somtthing. " + node.ToString());
            }
        }
    }

    private Vector3 removeY(Vector3 data)
    {
        return new Vector3(data.X, 0, data.Z);
    }

    private void OnVisibilityNotifierScreenExited()
    {
        EmitSignal(SignalName.BallLeavesScreen);
        QueueFree();
    }
}
