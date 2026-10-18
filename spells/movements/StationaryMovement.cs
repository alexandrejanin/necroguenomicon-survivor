using Godot;
using Godot.Collections;

[GlobalClass]
public partial class StationaryMovement : Movement
{
    // World-fixed at the spawn transform. Keeps the rotation set by the
    // scene or the pattern (AtRandomEnemy / AtHit place away from the
    // anchor and own the facing).
    public override void Setup(Projectile projectile, Player player, Vector2 spawnPos,
        int castIndex, int spawnIndex, int spawnTotal,
        Array<Node> mobs, Vector2 hit)
    {
        projectile.GlobalPosition = spawnPos;
        projectile.LinearVelocity = Vector2.Zero;
    }
}