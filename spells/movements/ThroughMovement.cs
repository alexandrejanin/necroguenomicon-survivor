using Godot;
using Godot.Collections;

[GlobalClass]
public partial class ThroughMovement : Movement
{
    // Flies straight along player->hit (Piercing dagger effect).
    [Export] public float Speed = 160.0f;

    public override void Setup(Projectile projectile, Player player, Vector2 spawnPos,
        int castIndex, int spawnIndex, int spawnTotal,
        Array<Node> mobs, Vector2 hit)
    {
        base.Setup(projectile, player, spawnPos, castIndex, spawnIndex, spawnTotal, mobs, hit);

        var dir = hit - player.GlobalPosition;

        if (dir.LengthSquared() < 0.001f)
            dir = hit - spawnPos;

        if (dir.LengthSquared() < 0.001f)
            dir = Vector2.Right;

        dir = dir.Normalized();
        projectile.Rotation = dir.Angle();
        projectile.LinearVelocity = dir * Speed;
    }
}