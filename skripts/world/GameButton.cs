using Godot;

public partial class GameButton : Node3D
{
    public void OnArea3dBodyEntered(Node3D body)
    {
        foreach (Node3D child in GetChildren())
        {
            if (child is ITriggerable triggerable)
            {
                triggerable.Trigger();
            }
        }
    }
}
