
using Godot;

public class ScoreManus : ItemBehavior
{
    private readonly DefaultLevel level;

    public ScoreManus(DefaultLevel level)
    {
        this.level = level;
    }

    public override void DoBehavior()
    {
        level.Score = Mathf.Max(0, level.Score - 35);
    }
}