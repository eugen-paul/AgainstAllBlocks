using System.Collections.Generic;
using Godot;

public partial class Achievements : Node
{
    private GodotObject ScriptObject;

    private IDictionary<int, List<string>> ACHIEVEMENTS;

    public override void _Ready()
    {
        GDScript script = GD.Load<GDScript>("res://scenes/localization/Localization.gd");
        ScriptObject = (GodotObject)script.New();

        ACHIEVEMENTS = new Dictionary<int, List<string>> {
        {  1, new List<string>(){GET_X_POINTS(10),        CATCH_X_ROCKETS(1),    CATCH_X_BOMBS(2)} },
        {  2, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        {  3, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        {  4, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        {  5, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        {  6, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        {  7, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        {  8, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        {  9, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 10, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 11, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 12, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 13, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 14, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 15, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 16, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 17, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 18, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 19, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        { 20, new List<string>(){GET_X_POINTS(10),        GET_X_POINTS(20),      GET_X_POINTS(30)} },
        };
    }

    private string GET_X_POINTS(int X)
    {
        return string.Format(var("GET_X_POINTS"), X);
    }

    private string CATCH_X_ROCKETS(int X)
    {
        return string.Format(var("CATCH_X_ROCKETS"), X);
    }

    private string CATCH_X_BOMBS(int X)
    {
        return string.Format(var("CATCH_X_BOMBS"), X);
    }

    private string var(string variableName)
    {
        return ScriptObject.Get(variableName).AsString();
    }

    public string GetAchivments(int level, int ball)
    {
        return ACHIEVEMENTS[level][ball];
    }
}
