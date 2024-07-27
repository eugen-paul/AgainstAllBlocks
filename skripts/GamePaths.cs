public sealed class GameScenePaths
{
    public const string MAIN_SCENE = "res://scenes/menu/Main.tscn";
    public const string GAME_SLOT_EMPTY = "res://scenes/hud/GameSlotEmpty.tscn";
    public const string GAME_SLOT_SAVE = "res://scenes/hud/GameSlotSave.tscn";

    private const string LEVEL_SCENE_PREFIX = "res://scenes/levels/Level";
    private const string LEVEL_SCENE_SUFFIX = ".tscn";


    public static string getLevelPath(int level)
    {
        return $"{LEVEL_SCENE_PREFIX}{level}{LEVEL_SCENE_SUFFIX}";
    }
}