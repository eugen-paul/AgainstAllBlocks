using System;
using Godot;

public partial class Upgrades : Control
{
    public Action CloseAction;

    private void OnOkButtonPressed()
    {
        CloseAction.Invoke();
    }
}
