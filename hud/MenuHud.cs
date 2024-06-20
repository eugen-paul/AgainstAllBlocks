using Godot;
using System;

public partial class MenuHud : CanvasLayer
{
    private void OnStartLoadGameButtonPressed()
    {
        var menu = ResourceLoader.Load<PackedScene>("res://levels/level1/Level1.tscn");
        GetTree().ChangeSceneToPacked(menu);
    }
}
