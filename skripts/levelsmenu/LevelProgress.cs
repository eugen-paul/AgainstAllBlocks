using System;
using Godot;

public partial class LevelProgress : Control
{

    private const string LEVEL_LABEL_PATH = "TextureButton/LevelLabel";
    private const string BALL_1_PATH = "TextureButton/HBoxContainer/Ball1";
    private const string BALL_2_PATH = "TextureButton/HBoxContainer/Ball2";
    private const string BALL_3_PATH = "TextureButton/HBoxContainer/Ball3";
    private const string START_BUTTON_PATH = "TextureButton/StartButton";

    private LevelProgressData levelProgressData = null;

    private event Action<int> StartLevelAction;

    public override void _Ready()
    {
        if (levelProgressData == null)
        {
            return;
        }
        GetNode<LevelLabel>(LEVEL_LABEL_PATH).SetLevel(levelProgressData.Level);

        var Achievements = GetNode<Achievements>("Achievements");

        GetNode<LevelBall>(BALL_1_PATH).TooltipText = Achievements.GetAchivments(levelProgressData.Level, 0);
        GetNode<LevelBall>(BALL_2_PATH).TooltipText = Achievements.GetAchivments(levelProgressData.Level, 1);
        GetNode<LevelBall>(BALL_3_PATH).TooltipText = Achievements.GetAchivments(levelProgressData.Level, 2);
        if (levelProgressData.Ball1)
        {
            GetNode<LevelBall>(BALL_1_PATH).SetGoldBall();
        }
        if (levelProgressData.Ball2)
        {
            GetNode<LevelBall>(BALL_2_PATH).SetGoldBall();
        }
        if (levelProgressData.Ball3)
        {
            GetNode<LevelBall>(BALL_3_PATH).SetGoldBall();
        }

        GetNode<Button>(START_BUTTON_PATH).Disabled = !levelProgressData.Reached;

        MouseFilter = MouseFilterEnum.Ignore;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        StartLevelAction = null;
    }

    public void Init(LevelProgressData levelProgressData, Action<int> startLevelAction)
    {
        this.StartLevelAction += startLevelAction;
        this.levelProgressData = levelProgressData;
    }

    private void OnStartButtonPressed()
    {
        StartLevelAction?.Invoke(levelProgressData.Level);
    }
}
