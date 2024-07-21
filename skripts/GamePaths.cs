public sealed class GameScenePaths
{
    public const string MAIN_SCENE = "res://scenes/menu/Main.tscn";

    private const string LEVEL_SCENE_PREFIX = "res://scenes/levels/Level";
    private const string LEVEL_SCENE_SUFFIX = ".tscn";

    public static string getLevelPath(int level)
    {
        return $"{LEVEL_SCENE_PREFIX}{level}{LEVEL_SCENE_SUFFIX}";
    }
}