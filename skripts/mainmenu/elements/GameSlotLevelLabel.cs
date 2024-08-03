using Godot;
using System;

public partial class GameSlotLevelLabel : Label
{
    public void SetLevel(int fps)
    {
        Text = "Level:" + fps.ToString();
    }
}
