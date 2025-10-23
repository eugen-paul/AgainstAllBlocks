using Godot;
using System;

public partial class ActorAnimationTree : AnimationTree
{
    [Export(PropertyHint.Range, "1,3,1")]
    private int do_idle = 1;

    [Export(PropertyHint.Range, "1,4,1")]
    private int primState = 1;
}
