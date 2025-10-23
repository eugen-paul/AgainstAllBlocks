using Godot;

public enum ActorPrimState : int
{
    Standing = 1,
    Walk = 2,
    Sitting = 3,
    Crouch = 4,
}

public enum IdleOneShot : int
{
    Idle_1 = 1,
    Idle_2 = 2,
    Idle_3 = 3,
}

[Tool]
public partial class ActorAnimationTree : AnimationTree
{
    private const string ONE_SHOT_IDLE_PATH = "parameters/StateMachinePrimState/Standing/OneShotIdle/request";

    private IdleOneShot doIdle = IdleOneShot.Idle_1;

    [Export]
    public IdleOneShot DoIdleEnum
    {
        get => doIdle;
        set
        {
            doIdle = value;
            DoOneShotIdle();
        }
    }

    [ExportToolButton("Play Idle!")]
    public Callable ClickMeButton => Callable.From(DoOneShotIdle);

    private ActorPrimState curentPrimState = ActorPrimState.Standing;

    [Export]
    public ActorPrimState PrimStateEnum
    {
        get => curentPrimState;
        set
        {
            curentPrimState = value;
            SetCurrentState();
        }
    }

    private void SetCurrentState()
    {
        AnimationNodeStateMachinePlayback stateMachine = (AnimationNodeStateMachinePlayback)Get("parameters/StateMachinePrimState/playback");

        switch (curentPrimState)
        {
            case ActorPrimState.Standing:
                Set(ONE_SHOT_IDLE_PATH, (int)AnimationNodeOneShot.OneShotRequest.Abort);
                stateMachine.Travel("Standing");
                break;
            case ActorPrimState.Walk:
                stateMachine.Travel("Walk");
                break;
            case ActorPrimState.Sitting:
                stateMachine.Travel("Sitting");
                break;
            case ActorPrimState.Crouch:
                stateMachine.Travel("Crouch");
                break;
            default:
                GD.PrintErr("ActorAnimationTree: Illegal State Value: " + curentPrimState.ToString());
                break;
        }
    }

    private void DoOneShotIdle()
    {
        Set(ONE_SHOT_IDLE_PATH, (int)AnimationNodeOneShot.OneShotRequest.Fire);
    }
}
