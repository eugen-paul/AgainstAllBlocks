using Godot;

public partial class BgBlocks : Node3D, ITriggerable
{
    public void Trigger()
    {
        var blocks = FindChildren("Bonus*");
        if (blocks.Count == 0) return;
        var randomBlock = (Block)blocks[(int)(GD.Randi() % blocks.Count)];
        randomBlock.Hit(5);

        var blockName = randomBlock.Name.ToString();
        var blockNummer = blockName[(blockName.Length - 1)..];
        var explosion = GetNode<GpuParticles3D>("Fire" + blockNummer);
        explosion.Emitting = true;
        
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }

}
