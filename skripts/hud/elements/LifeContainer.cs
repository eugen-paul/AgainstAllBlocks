
using Godot;

public partial class LifeContainer : HBoxContainer
{

    private PackedScene HeartAnimPath { get; set; } = ResourceLoader.Load<PackedScene>("res://scenes/hud/Heart.tscn");

    private const int MAX_LIFES = 5;

    public void SetLifes(int lifes)
    {
        if (lifes < 0)
        {
            return;
        }

        var currentLifes = GetChildCount();
        if (lifes > currentLifes && lifes < MAX_LIFES)
        {
            for (int i = currentLifes; i < lifes; i++)
            {
                Heart heart = HeartAnimPath.Instantiate<Heart>();
                AddChild(heart);
                heart.PlayCreate();
            }
        }
        else if (lifes < currentLifes)
        {
            for (int i = currentLifes; i > lifes; i--)
            {
                Heart heart = (Heart)GetChild(0);
                heart.PlayDestroy(() => RemoveChild(GetChild(0)));
            }
        }
    }

}