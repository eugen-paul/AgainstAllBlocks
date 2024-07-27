using System.Diagnostics;
using Godot;

public sealed class PreferencePaths
{
    public static readonly string CONFIG_DIR_PATH = "user://";

    public static void MkDir()
    {
        var dir = DirAccess.Open(CONFIG_DIR_PATH);
        var error = dir.MakeDirRecursive(CONFIG_DIR_PATH);
        if (error != Error.Ok)
        {
            Debug.Print("Can't create User folder: " + CONFIG_DIR_PATH);
            return;
        }
    }
}