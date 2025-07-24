using Godot;

public partial class Heart : Control
{
    public delegate void AnimationFinishedCallBack();
    private AnimatedSprite2D sprite;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("Sprite");
        PlayIdle();
    }

    public void PlayIdle()
    {
        sprite.Play("idle");
    }

    public void PlayDestroy(AnimationFinishedCallBack onAnimationEnd = null)
    {
        if (sprite.Animation == "destroy")
        {
            return;
        }

        sprite.Play("destroy");
        if (onAnimationEnd != null)
        {
            sprite.AnimationFinished += () => onAnimationEnd();
        }
    }

    public void PlayCreate(AnimationFinishedCallBack onAnimationEnd = null)
    {
        sprite.Play("create");
        sprite.AnimationFinished += () =>
        {
            PlayIdle();
            onAnimationEnd?.Invoke();
        };
    }
}
