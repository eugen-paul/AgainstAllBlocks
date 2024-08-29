using Godot;

public enum ItemType
{
    NONE = 0,
    LIFE_ADD,
    LIFE_REMOVE,
    BALL_ADD,
    BALL_SPEED_INCREASE,
    BALL_SPEED_DECREASE,
    BALL_SPEED_DIRECTION_RANDOM,
    PADDLE_SPEED_INCREASE,
    PADDLE_SPEED_DECREASE,
    PADDLE_SPEED_STOP,
    SCORE_DOUBLING,
    SCORE_HALVING,
    SCORE_DELETION,
}

public class ItemBehaviorFactory
{
    private ItemBehaviorFactory() { }

    public static ItemBehavior Create(ItemType type, AbstractLevel level)
    {
        return type switch
        {
            ItemType.LIFE_ADD => new LifeBehavior(1, level),
            ItemType.LIFE_REMOVE => new LifeBehavior(-1, level),
            _ => new EmptyItemBehavior(),
        };
    }
}

public abstract class ItemBehavior
{
    public abstract void DoBehavior();
}

public class EmptyItemBehavior : ItemBehavior
{
    public override void DoBehavior() { }
}

public partial class Item : CharacterBody3D
{
    public ItemType ItemType { get; set; } = ItemType.NONE;

    public AbstractLevel Level { get; set; } = null;

    [Export]
    public int ItemSpeed { get; set; } = 10;

    public override void _EnterTree()
    {
        Velocity = Vector3.Back * ItemSpeed;
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    public void OnVisibilityNotifierScreenExited()
    {
        Level.ItemDestroyd(this);
        QueueFree();
    }

    public void OnPaddleDetectorBodyEntered(Node3D paddle)
    {
        Level.ItemDestroyd(this);
        ItemBehaviorFactory.Create(ItemType, Level)?.DoBehavior();
        QueueFree();
    }
}
