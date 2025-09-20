using Godot;

public partial class Goal : Node3D, ITriggerable
{

    public override void _Ready()
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Finished += StopAnimation;
    }

    public void Trigger()
    {
        var blocks = FindChildren("Block*");
        if (blocks.Count == 0) return;

        var firstBlock = (Block)blocks[0];
        firstBlock.Hit(1);
        var blockName = firstBlock.Name.ToString();
        var blockNummer = blockName[(blockName.Length - 1)..];
        var explosion = GetNode<GpuParticles3D>("Fire" + blockNummer);
        explosion.Emitting = true;

        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();

        StartAnimation();
    }

    private void StartAnimation()
    {
        var bg = GetNodeOrNull<Node>("/root/Level15/Background");
        if (bg == null)
        {
            return;
        }
        var children = bg.GetChildren();
        foreach (var child in children)
        {
            if (child is Node3D)
            {
                var cc = child.GetChildren();
                foreach (var c in cc)
                {
                    if (c is AnimatedSprite3D ap)
                    {
                        ap.Play("Goal");
                    }
                }
            }
        }
    }

    private void StopAnimation()
    {
        var bg = GetNodeOrNull<Node>("/root/Level15/Background");
        if (bg == null)
        {
            return;
        }
        var children = bg.GetChildren();
        foreach (var child in children)
        {
            if (child is Node3D)
            {
                var cc = child.GetChildren();
                foreach (var c in cc)
                {
                    if (c is AnimatedSprite3D ap)
                    {
                        ap.Play("Idle");
                    }
                }
            }
        }
    }

}
