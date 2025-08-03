using System.Diagnostics;
using Godot;

public partial class GameButton : Node3D
{
    public void OnArea3dBodyEntered(Node3D body)
    {
        Debug.Print("Button pressed by: " + body.Name);

        foreach (Node3D child in GetChildren())
        {
            if (child is ITriggerable triggerable)
            {
                triggerable.Trigger();
            }
        }
    }
}
