using Godot;

public partial class Goal : Node3D, ITriggerable
{
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
    }

}
