using Godot;
using Godot.Collections;

[GlobalClass]
public partial class RadialMovement : Movement
{
    // Faces outward from the cast anchor (angle spread across the cast),
    // then flies straight. Speed 0 = stationary facing outward
    // (Tentacle circle).
    [Export] public float Speed;

    public override void Setup(Projectile projectile, Player player, Vector2 spawnPos,
        int castIndex, int spawnIndex, int spawnTotal,
        Array<Node> mobs, Vector2 hit)
    {
        base.Setup(projectile, player, spawnPos, castIndex, spawnIndex, spawnTotal, mobs, hit);
        var angle = Mathf.Tau * spawnIndex / Mathf.Max(1, spawnTotal);
        projectile.Rotation = angle;
        if (Mathf.IsZeroApprox(Speed))
            projectile.LinearVelocity = Vector2.Zero;
        else
            projectile.LinearVelocity = Vector2.Right.Rotated(angle) * Speed;
    }
}