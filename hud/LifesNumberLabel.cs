using Godot;
using System;

public partial class LifesNumberLabel : Label
{
    public void SetLifes(int lifes)
    {
        Text = lifes.ToString();
    }
}
