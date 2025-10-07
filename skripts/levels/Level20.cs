using Godot;

public enum LevelState
{
    NotStarted,
    DoorOpening,
    ArmeeMoving,
    ArmeeOnField,
    LevelDone
}

public partial class Level20 : DefaultLevel
{
    private const string DOOR_PATH = "AllBlocks/CaslteDoor";
    private CaslteDoor door;

    private LevelState state = LevelState.NotStarted;
    private int currentPack = 1;
    private int maxPacks = 3;

    public override void _Ready()
    {
        base._Ready();
        door = GetNode<CaslteDoor>(DOOR_PATH);
        door.Init(OnDoorOpened, OnDoorClosed);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        switch (state)
        {
            case LevelState.NotStarted:
                break;
            case LevelState.ArmeeMoving:
                DoArmeeMoving(delta);
                break;
            case LevelState.ArmeeOnField:
                CheckCurrentPack();
                break;
        }
    }

    protected override void StartRound()
    {
        base.StartRound();
        state = LevelState.ArmeeMoving;
    }

    protected override void LevelDone()
    {
        base.LevelDone();
        state = LevelState.LevelDone;
    }

    protected override void GameOver()
    {
        base.GameOver();
        state = LevelState.LevelDone;
    }

    private void OnDoorOpened()
    {
        state = LevelState.ArmeeMoving;
    }

    private void OnDoorClosed()
    {

    }

    private void DoArmeeMoving(double delta)
    {
        var packChildren = GetNode<Node3D>("AllBlocks/Pack" + currentPack).GetChildren();
        bool allDone = true;
        foreach (var child in packChildren)
        {
            if (child.GetChildOrNull<PathFollow3D>(0) is PathFollow3D pathFollow3D)
            {
                pathFollow3D.Progress += (float)(3 * delta);
                if (pathFollow3D.ProgressRatio < 1)
                {
                    allDone = false;
                }
            }
        }

        if (allDone)
        {
            door.Close();
            state = LevelState.ArmeeOnField;
        }
    }

    private void CheckCurrentPack()
    {
        if (currentPack > maxPacks)
        {
            state = LevelState.LevelDone;
            return;
        }

        var paths = GetNode<Node3D>("AllBlocks/Pack" + currentPack).GetChildren();
        bool allDone = true;
        foreach (var child in paths)
        {
            if (child.GetNodeOrNull<Node3D>("PathFollow3D/Node3D") is Node3D blocks)
            {
                if (blocks.GetChildren().Count > 0)
                {
                    allDone = false;
                    break;
                }
            }
        }

        if (allDone)
        {
            currentPack++;
            door.Open();
            state = LevelState.DoorOpening;
        }
    }

}
