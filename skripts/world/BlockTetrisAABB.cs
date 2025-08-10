using Godot;

[Tool]
public partial class BlockTetrisAABB : BlockTetris
{
    public override Aabb GetGlobalAABB()
    {
        var collisionShape = GetNode<CollisionShape3D>("CollisionShape3DAABB");
        if (collisionShape == null)
        {
            return new Aabb();
        }
        
        if (collisionShape.Shape is not BoxShape3D boxShape1)
        {
            GD.PushError("CollisionShape3D does not have a BoxShape3D shape.");
            return new Aabb();
        }

        var size = boxShape1.Size;
        var center = collisionShape.GlobalPosition;
        return new Aabb(center - size / 2, size);
    }

}
