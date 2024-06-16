using Godot;
using System;

public partial class Block : CharacterBody3D
{

    [Signal]
    public delegate void BlockDestroyedEventHandler();

    public void Hit()
    {
        EmitSignal(SignalName.BlockDestroyed);
        QueueFree();
    }
}
