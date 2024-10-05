
using Godot;

public class ScoreManus : ItemBehavior
{
    private readonly AbstractLevel level;

    public ScoreManus(AbstractLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.Score = Mathf.Max(0, level.Score - 35);
    }
}