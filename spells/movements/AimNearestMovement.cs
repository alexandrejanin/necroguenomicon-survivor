using Godot;
using Godot.Collections;

[GlobalClass]
public partial class AimNearestMovement : Movement
{
    // Flies straight at the nearest mob sampled on setup (Crystal).
    [Export] public float Speed = 160.0f;

    public override void Setup(Projectile projectile, Player player, Vector2 spawnPos,
        int castIndex, int spawnIndex, int spawnTotal,
        Array<Node> mobs, Vector2 hit)
    {
        base.Setup(projectile, player, spawnPos, castIndex, spawnIndex, spawnTotal, mobs, hit);
        var dir = Facing(player, spawnPos, mobs, spawnIndex, spawnTotal);
        projectile.Rotation = dir.Angle();
        projectile.LinearVelocity = dir * Speed;
    }

    private Vector2 Facing(Player player, Vector2 spawnPos,
        Array<Node> mobs, int spawnIndex, int spawnTotal)
    {
        Mob best = null;
        var bestDistance = Mathf.Inf;
        foreach (var mob in mobs)
        {
            if (mob is not Mob m || !IsInstanceValid(m)) continue;
            var distance = spawnPos.DistanceSquaredTo(m.GlobalPosition);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = m;
            }
        }

        if (best != null)
        {
            var toMob = best.GlobalPosition - spawnPos;
            if (toMob.LengthSquared() > 0.001f) return toMob.Normalized();
        }

        return Vector2.Right.Rotated(Mathf.Tau * spawnIndex / Mathf.Max(1, spawnTotal));
    }
}