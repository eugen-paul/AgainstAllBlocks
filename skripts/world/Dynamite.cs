using Godot;
using System.Diagnostics;

public partial class Dynamite : Node3D, ITriggerable
{
    public void Trigger()
    {
        Debug.Print("Dynamite triggered" + Name);
        QueueFree();
    }

}
