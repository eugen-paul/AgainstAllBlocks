using System;
using System.Collections.Generic;
using Godot;

public partial class Level1 : Node
{
    [Export]
    public PackedScene BallScene { get; set; }

    [Export]
    public int Lifes { get; set; } = 5;

    private int _ballsCount = 0;
    private CharacterBody3D _paddle;

    private readonly LinkedList<Ball> _ballsOnPaddle = new();

    public bool Pause { get; set; } = false;

    private float movePositionX = 0f;

    public override void _Ready()
    {

        _paddle = GetNode<CharacterBody3D>("Paddle");
        AddBall();
    }

    private void AddBall()
    {
        var ball = BallScene.Instantiate<Ball>();
        _ballsCount++;
        ball.Position = new Vector3(_paddle.Position.X, 1, _paddle.Position.Z - 1);

        _ballsOnPaddle.AddLast(ball);
        AddChild(ball);
    }

    public override void _PhysicsProcess(double delta)
    {
        var paddlePos3D = _paddle.Position;
        var paddlePos2dX = GetViewport().GetCamera3D().UnprojectPosition(paddlePos3D).X;

        var viewportSizeX = GetViewport().GetVisibleRect().Size.X;
        // var mousePosX = GetViewport().GetMousePosition().X;
        var mousePosX = movePositionX;

        float sceeneSizeX = 22f;

        var paddleMoveX = mousePosX - paddlePos2dX;

        var d = Math.Abs(paddleMoveX) * 100f / viewportSizeX;
        var dx = d * sceeneSizeX / 100f;

        if (paddleMoveX > 0)
        {
            _paddle.MoveAndCollide(new Vector3(dx, 0, 0));
        }
        else if (paddleMoveX < 0)
        {
            _paddle.MoveAndCollide(new Vector3(-dx, 0, 0));
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("shoot") && !Pause && _ballsOnPaddle.Count > 0)
        {
            var ball = _ballsOnPaddle.First.Value;
            _ballsOnPaddle.RemoveFirst();
            ball.Velocity = Vector3.Forward * ball.Speed;
        }
        else if (@event is InputEventMouseMotion eventMouseMotion)
        {
            movePositionX = eventMouseMotion.Position.X;
        }
    }
}
