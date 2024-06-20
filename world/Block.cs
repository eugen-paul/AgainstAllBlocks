using Godot;
using System;

public partial class Block : CharacterBody3D
{

    [Signal]
    public delegate void BlockDestroyedEventHandler(int scoreBonus = 0);

    public void Hit(int scoreBonus)
    {
        EmitSignal(SignalName.BlockDestroyed, scoreBonus);
        QueueFree();
    }
}
