using Godot;
using System;

public partial class CarOnPath3dBody : StaticBody3D, Hitable
{

    private Action _changeDirectionCallback;

    public void SetChangeDirectionCallback(Action changeDirectionCallback)
    {
        _changeDirectionCallback = changeDirectionCallback;
    }

    public void Hit(int scoreBonus, int hitPower = 1)
    {
        _changeDirectionCallback?.Invoke();
    }

}
