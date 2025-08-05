using Godot;
using System;

public partial class CarOnPath3dBody : StaticBody3D, Hitable
{

    private Action _hitCarCallback;

    public void SetHitCarCallback(Action hitCarCallback)
    {
        _hitCarCallback = hitCarCallback;
    }

    public void Hit(int scoreBonus, int hitPower = 1)
    {
        _hitCarCallback?.Invoke();
    }

}
