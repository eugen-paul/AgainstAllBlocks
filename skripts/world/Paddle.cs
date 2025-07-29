using System;
using Godot;

public partial class Paddle : CharacterBody3D
{
    private static readonly string SPRITE_PATH = "Sprite3D";
    private static readonly string PROGRESSBAR_PATH = "Sprite3D/SubViewport/ProgressBar";

    [Export]
    public float MaxPaddleSpeed { get; set; } = 30.0f;

    private float bonusSpeed = 0.0f;

    public bool Pause { get; set; } = false;

    private float movePositionX = 0f;

    private readonly ICollisionBallPaddle collisionBallPaddle = new CollisionBallPaddle();

    private bool isStopped = false;
    private DateTime stopTime = DateTime.MinValue;
    private int stopDuration = 5000; // 5 seconds

    public override void _Ready()
    {
        GetNode<Sprite3D>(SPRITE_PATH).Texture = GetNode<SubViewport>("Sprite3D/SubViewport").GetTexture();
        GetNode<ProgressBar>(PROGRESSBAR_PATH).Visible = false;
    }

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
                PlayHitBAll();
            }
        }
        
        if (isStopped)
        {
            ProgressBar progressBar = GetNode<ProgressBar>(PROGRESSBAR_PATH);
            if ((DateTime.Now - stopTime).TotalMilliseconds > stopDuration)
            {
                isStopped = false;
                bonusSpeed = 0f;
                progressBar.Visible = false;
            }
            else
            {
                bonusSpeed = -MaxPaddleSpeed;
                progressBar.Value = 100f - ((float)((DateTime.Now - stopTime).TotalMilliseconds / stopDuration))*100f;
                progressBar.Visible = true;
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

        var maxXVelocity = Mathf.Max(0f, (MaxPaddleSpeed + bonusSpeed) * (float)delta);

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

    public void AddSpeed(float value)
    {
        bonusSpeed += value;
    }

    public void RemoveSpeed()
    {
        isStopped = true;
        stopTime = DateTime.Now;
    }

    public void Reset()
    {
        bonusSpeed = 0;
        isStopped = false;
        stopTime = DateTime.MinValue;
        GetNode<ProgressBar>(PROGRESSBAR_PATH).Visible = false;
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

    public void PlayHitBAll()
    {
        GetNode<AudioStreamPlayer>("HitBall").Play();
    }

    public void PlayCatchItem()
    {
        GetNode<AudioStreamPlayer>("CatchItem").Play();
    }

    public void PlayEvilLaugh()
    {
        GetNode<AudioStreamPlayer>("EvilLaugh").Play();
    }

    public void PlayLoseBall()
    {
        GetNode<AudioStreamPlayer>("LoseBall").Play();
    }

    public void PlayLoseLife()
    {
        GetNode<AudioStreamPlayer>("LoseLife").Play();
    }

    public void PlayCoinPickup()
    {
        GetNode<AudioStreamPlayer>("CoinPickup").Play();
    }
}
