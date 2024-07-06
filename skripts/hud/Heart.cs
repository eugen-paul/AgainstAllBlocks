using System.Diagnostics;
using Godot;

public partial class Heart : Control
{
    public delegate void AnimationFinishedCallBack();

    public override void _Ready()
    {
        PlayIdle();
    }

    public void PlayIdle()
    {
        GetNode<AnimatedSprite2D>("Sprite").Play("idle");
    }

    public void PlayDestroy(AnimationFinishedCallBack onAnimationEnd = null)
    {
        GetNode<AnimatedSprite2D>("Sprite").Play("destroy");
        if (onAnimationEnd != null)
        {
            GetNode<AnimatedSprite2D>("Sprite").AnimationFinished += () => onAnimationEnd();
        }
    }

    public void PlayCreate(AnimationFinishedCallBack onAnimationEnd = null)
    {
        GetNode<AnimatedSprite2D>("Sprite").Play("create");
        if (onAnimationEnd != null)
        {
            GetNode<AnimatedSprite2D>("Sprite").AnimationFinished += () => onAnimationEnd();
        }
    }
}
