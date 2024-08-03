using System.Collections.Generic;
using System.Collections.Immutable;

public sealed class GameScenePaths
{
    public const string MAIN_SCENE = "res://scenes/mainmenu/Main.tscn";
    public const string LEVEL_SELECTION_SCENE = "res://scenes/levelsmenu/Levels.tscn";
    private const string LEVEL_SCENE_PREFIX = "res://scenes/levels/Level";
    private const string LEVEL_SCENE_SUFFIX = ".tscn";
    public const string DEFAULT_BALL_SCENE = "res://scenes/world/Ball.tscn";
    public const string DEFAULT_ARROW_SCENE = "res://scenes/world/Arrow.tscn";

    public static string GetLevelPath(int level)
    {
        return $"{LEVEL_SCENE_PREFIX}{level}{LEVEL_SCENE_SUFFIX}";
    }

    public readonly static ImmutableDictionary<int, string> LEVELS = new Dictionary<int, string> {
        {  1, GetLevelPath( 1) },
        {  2, GetLevelPath( 2) },
        {  3, GetLevelPath( 3) },
        {  4, GetLevelPath( 4) },
        {  5, GetLevelPath( 5) },
        {  6, GetLevelPath( 6) },
        {  7, GetLevelPath( 7) },
        {  8, GetLevelPath( 8) },
        {  9, GetLevelPath( 9) },
        { 10, GetLevelPath(10) },
        { 11, GetLevelPath(11) },
        { 12, GetLevelPath(12) },
        { 13, GetLevelPath(13) },
        { 14, GetLevelPath(14) },
        }
        .ToImmutableDictionary();
}