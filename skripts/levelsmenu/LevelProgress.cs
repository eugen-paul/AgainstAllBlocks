using Godot;

public partial class LevelProgress : Control
{

    private const string LEVEL_LABEL_PATH = "TextureButton/LevelLabel";
    private const string BALL_1_PATH = "TextureButton/HBoxContainer/Ball1";
    private const string BALL_2_PATH = "TextureButton/HBoxContainer/Ball2";
    private const string BALL_3_PATH = "TextureButton/HBoxContainer/Ball3";
    private const string START_BUTTON_PATH = "TextureButton/StartButton";

    private LevelProgressData levelProgressData = null;

    public override void _Ready()
    {
        if (levelProgressData == null)
        {
            return;
        }
        GetNode<LevelLabel>(LEVEL_LABEL_PATH).SetLevel(levelProgressData.Level);

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

    public void Init(LevelProgressData levelProgressData)
    {
        this.levelProgressData = levelProgressData;
    }

    private void OnStartButtonPressed()
    {
        var next = ResourceLoader.Load<PackedScene>(GameScenePaths.GetLevelPath(levelProgressData.Level), cacheMode : ResourceLoader.CacheMode.IgnoreDeep);
        GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToPacked, next);
    }
}
