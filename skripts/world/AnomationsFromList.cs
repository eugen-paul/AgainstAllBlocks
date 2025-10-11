using Godot;
using Godot.Collections;

public partial class AnomationsFromList : Node3D
{
    [Export]
    public AnimationPlayer AnimationPlayer { get; set; } = null!;

    [Export]
    public Array<string> AnimationNames { get; set; } = [];

    [Export]
    public float TimeBetweenAnomations { get; set; } = 1.0f;

    [Export]
    public bool RandomOrder { get; set; } = false;

    private double timeToNextAnomation = 0;
    private int currentAnomationIndex = -1;

    public override void _Ready()
    {
        if (AnimationNames.Count == 0)
        {
            return;
        }
        PlayNextAnomation();
    }

    public override void _Process(double delta)
    {
        if (AnimationNames.Count == 0 || AnimationPlayer.IsPlaying())
        {
            return;
        }
        timeToNextAnomation -= delta;
        if (timeToNextAnomation <= 0)
        {
            PlayNextAnomation();
        }
    }

    private void PlayNextAnomation()
    {
        if (RandomOrder)
        {
            int nextIndex = GD.RandRange(0, AnimationNames.Count - 1);
            currentAnomationIndex = nextIndex;
        }
        else
        {
            currentAnomationIndex = (currentAnomationIndex + 1) % AnimationNames.Count;
        }
        
        if (!AnimationPlayer.HasAnimation(AnimationNames[currentAnomationIndex]))
        {
            GD.PrintErr($"Animation {AnimationNames[currentAnomationIndex]} not found in AnimationPlayer");
            return;
        }
        
        AnimationPlayer.Play(AnimationNames[currentAnomationIndex]);
        timeToNextAnomation = TimeBetweenAnomations;
    }
}
