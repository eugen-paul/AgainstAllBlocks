using Godot;

public partial class LifeContainer : Panel
{
    private static readonly string HEART_CONTAINER_5_PATH = "Container5";
    private static readonly string HEART_CONTAINER_9_PATH = "Container9";
    private static readonly string HEART_CONTAINER_18_PATH = "Container18";

    private PackedScene HeartAnimPath { get; set; } = ResourceLoader.Load<PackedScene>("res://scenes/hud/Heart.tscn");

    private int currentLifes = 0;

    public override void _Ready()
    {
        RemoveAllChildren(HEART_CONTAINER_5_PATH);
        RemoveAllChildren(HEART_CONTAINER_9_PATH);
        RemoveAllChildren(HEART_CONTAINER_18_PATH);
    }

    private void RemoveAllChildren(string containerPath)
    {
        foreach (var child in GetNode<Node>(containerPath).GetChildren())
        {
            GetNode<Node>(containerPath).RemoveChild(child);
            child.QueueFree();
        }
    }

    private void AddNewChild(string containerPath)
    {
        Heart heart = HeartAnimPath.Instantiate<Heart>();
        GetNode<Node>(containerPath).AddChild(heart);
        heart.PlayCreate();
    }

    private void RemoveFirstChild(string containerPath)
    {
        Heart heart = (Heart)GetNode<Control>(containerPath).GetChild(0);
        heart.PlayDestroy(heart.QueueFree);
    }

    public void SetLifes(int lifes)
    {
        if (lifes < 0)
        {
            currentLifes = 0;
            RemoveAllChildren(HEART_CONTAINER_5_PATH);
            RemoveAllChildren(HEART_CONTAINER_9_PATH);
            RemoveAllChildren(HEART_CONTAINER_18_PATH);
            return;
        }

        if (currentLifes < lifes)
        {
            while (currentLifes < lifes)
            {
                AddNewChild(HEART_CONTAINER_5_PATH);
                AddNewChild(HEART_CONTAINER_9_PATH);
                AddNewChild(HEART_CONTAINER_18_PATH);
                currentLifes++;
            }
        }
        else if (lifes < currentLifes)
        {
            while (currentLifes > lifes)
            {
                RemoveFirstChild(HEART_CONTAINER_5_PATH);
                RemoveFirstChild(HEART_CONTAINER_9_PATH);
                RemoveFirstChild(HEART_CONTAINER_18_PATH);
                currentLifes--;
            }
        }

        if (currentLifes <= 4)
        {
            GetNode<Control>(HEART_CONTAINER_5_PATH).Visible = true;
            GetNode<Control>(HEART_CONTAINER_9_PATH).Visible = false;
            GetNode<Control>(HEART_CONTAINER_18_PATH).Visible = false;
        }
        else if (currentLifes <= 9)
        {
            GetNode<Control>(HEART_CONTAINER_5_PATH).Visible = false;
            GetNode<Control>(HEART_CONTAINER_9_PATH).Visible = true;
            GetNode<Control>(HEART_CONTAINER_18_PATH).Visible = false;
        }
        else
        {
            GetNode<Control>(HEART_CONTAINER_5_PATH).Visible = false;
            GetNode<Control>(HEART_CONTAINER_9_PATH).Visible = false;
            GetNode<Control>(HEART_CONTAINER_18_PATH).Visible = true;
        }
    }
}