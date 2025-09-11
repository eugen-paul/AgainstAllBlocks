using System.Linq;
using Godot;

public partial class GameButton : Node3D
{
    public void OnArea3dBodyEntered(Node3D body)
    {
        if (body is not Ball)
        {
            return;
        }

        foreach (Node3D child in GetChildren().Cast<Node3D>())
        {
            if (child is ITriggerable triggerable)
            {
                triggerable.Trigger();
            }
        }
    }
}
