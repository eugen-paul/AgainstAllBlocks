using Godot;

[Tool]
public partial class Path3dMultiObjects : Path3D
{
    private const string PATHFOLLOW3D_CHILD_NAME = "PathFollow3D";
    private const string CHILDS_TO_MOVE_NAME = "ToMove";

    public override void _Ready()
    {
        base._Ready();
        InitObjectsOnPath();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        MoveAllObjectsOnPath(delta);
    }

    private void InitObjectsOnPath()
    {
        var toMoveObjects = GetNodeOrNull<Node>(CHILDS_TO_MOVE_NAME);
        var pathFollow = GetNodeOrNull<PathFollow3D>(PATHFOLLOW3D_CHILD_NAME);
        if (toMoveObjects == null || pathFollow == null)
        {
            return;
        }

        var childCnt = toMoveObjects.GetChildCount();
        for (int i = 0; i < childCnt; i++)
        {
            var child3d = toMoveObjects.GetChildOrNull<Node3D>(0);
            if (child3d == null)
            {
                continue;
            }
            if (pathFollow.Duplicate() is PathFollow3D newPathFollow)
            {
                toMoveObjects.RemoveChild(child3d);
                newPathFollow.AddChild(child3d);
                child3d.Owner = newPathFollow;
                AddChild(newPathFollow);
                newPathFollow.Owner = this;
                newPathFollow.Name = $"{PATHFOLLOW3D_CHILD_NAME}_{i}";
                newPathFollow.ProgressRatio = (float)i / (float)childCnt;
            }
        }
    }

    private void MoveAllObjectsOnPath(double delta)
    {
        var childCnt = GetChildCount();
        for (int i = 0; i < childCnt; i++)
        {
            if (GetChildOrNull<PathFollow3D>(i) is PathFollow3D pathFollow)
            {
                pathFollow.Progress += (float)(delta * 2.0);
                if (pathFollow.Progress >= GetCurve().GetBakedLength())
                {
                    pathFollow.Progress = 0;
                }
            }
        }
    }
}
