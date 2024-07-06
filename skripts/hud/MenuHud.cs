using Godot;
using System;

public partial class MenuHud : CanvasLayer
{
    private void OnStartLoadGameButtonPressed()
    {
        var menu = ResourceLoader.Load<PackedScene>("res://scenes/levels/Level1.tscn");
        GetTree().ChangeSceneToPacked(menu);
    }
}
