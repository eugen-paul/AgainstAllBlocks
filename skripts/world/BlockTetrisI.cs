using Godot;

[Tool]
public partial class BlockTetrisI : ABlock, ContainItem
{
    public override void _Ready()
    {
        EditDamageEffect();
        ShowItem();
    }

    protected override void EditDamageEffect() { }
}
