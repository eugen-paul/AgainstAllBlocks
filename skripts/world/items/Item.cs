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
    ROCKET,
    BOMBS,
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
            ItemType.ROCKET => new RocketBehavior(item, level),
            ItemType.BOMBS => new BombsBehavior(item, level),
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
public partial class Item : Node3D
{
    public ItemType _itemType = ItemType.NONE;

    private Vector3 velocity = Vector3.Back;

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
    public float ItemSpeedZ { get; set; } = 10;

    [Export]
    public float ItemSpeedX { get; set; } = 0;

    [Export]
    public float ItemDecelerationX { get; set; } = 0;

    public override void _Ready()
    {
        velocity = new Vector3(ItemSpeedX, 0, ItemSpeedZ);
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
        if (Engine.IsEditorHint())
        {
            return;
        }
        if (velocity.X > 0)
        {
            velocity.X -= (float)delta * ItemDecelerationX;
        }
        else
        {
            velocity.X += (float)delta * ItemDecelerationX;
        }
        Position += velocity * (float)delta;
    }

    public void OnVisibilityNotifierScreenExited()
    {
        Level.TemporaryDestroyd(this);
        QueueFree();
    }

    public void OnPaddleDetectorBodyEntered(Node3D paddle)
    {
        Level.TemporaryDestroyd(this);
        ItemBehaviorFactory.Create(ItemType, this, Level)?.DoBehavior();
        QueueFree();
    }
}
