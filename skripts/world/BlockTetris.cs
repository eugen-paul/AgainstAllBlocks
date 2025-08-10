using Godot;

[Tool]
public partial class BlockTetris : ABlock, ContainItem
{
    public override void _Ready()
    {
        EditDamageEffect();
        ShowItem();
    }

    protected override void EditDamageEffect() { }

}
