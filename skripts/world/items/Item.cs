using Godot;

public enum ItemType
{
    NONE = 0,
    LIFE_ADD,
    LIFE_REMOVE,
    DEATH,
    BALL_ADD,
    BALL_SPEED_INCREASE,
    BALL_SPEED_DECREASE,
    BALL_DIRECTION_RANDOM,
    PADDLE_SPEED_INCREASE,
    PADDLE_SPEED_DECREASE,
    SCORE_BONUS,
    SCORE_MANUS,
}

public class ItemBehaviorFactory
{
    private ItemBehaviorFactory() { }

    public static ItemBehavior Create(ItemType type, Item item, AbstractLevel level)
    {
        return type switch
        {
            ItemType.LIFE_ADD => new LifeBehavior(1, level),
            ItemType.LIFE_REMOVE => new LifeBehavior(-1, level),
            ItemType.DEATH => new DeathBehavior(level),
            ItemType.BALL_ADD => new BallAddBehavior(item, level),
            ItemType.BALL_SPEED_INCREASE => new BallSpeedIncrease(level),
            ItemType.BALL_SPEED_DECREASE => new BallSpeedDecrease(level),
            ItemType.BALL_DIRECTION_RANDOM => new BallDirectionRandom(level),
            ItemType.PADDLE_SPEED_INCREASE => new PaddleSpeedIncrease(level),
            ItemType.PADDLE_SPEED_DECREASE => new PaddleSpeedDecrease(level),
            ItemType.SCORE_BONUS => new ScoreBonus(level),
            ItemType.SCORE_MANUS => new ScoreManus(level),
            _ => new EmptyItemBehavior(),
        };
    }

    public static string getIconPath(ItemType type)
    {
        return "res://assets/textures/world/Items-" + type + ".png";
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

[Tool]
public partial class Item : CharacterBody3D
{
    public ItemType _itemType = ItemType.NONE;

    [Export]
    public ItemType ItemType
    {
        get => _itemType;
        set
        {
            _itemType = value;
            RefreshTexture();
        }
    }

    public AbstractLevel Level { get; set; } = null;

    [Export]
    public int ItemSpeed { get; set; } = 10;

    public override void _EnterTree()
    {
        Velocity = Vector3.Back * ItemSpeed;
    }

    public override void _Ready()
    {
        RefreshTexture();
    }

    private void RefreshTexture()
    {
        var texture = GD.Load<Texture2D>(ItemBehaviorFactory.getIconPath(ItemType));
        if (GetNodeOrNull<Sprite3D>("Sprite3D") != null)
        {
            GetNode<Sprite3D>("Sprite3D").Texture = texture;
        }
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
        ItemBehaviorFactory.Create(ItemType, this, Level)?.DoBehavior();
        QueueFree();
    }
}
