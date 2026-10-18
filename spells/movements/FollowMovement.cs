using Godot;
using Godot.Collections;

[GlobalClass]
public partial class FollowMovement : Movement
{
    // Glued to the player; faces nearest mob once on spawn, then swings.
    // Forward reach lives in the visual scene (sprite/shape offset).
    [Export] public float SwingAngle;

    public override void Setup(Projectile projectile, Player player, Vector2 spawnPos,
        int castIndex, int spawnIndex, int spawnTotal,
        Array<Node> mobs, Vector2 hit)
    {
        base.Setup(projectile, player, spawnPos, castIndex, spawnIndex, spawnTotal, mobs, hit);
        projectile.GlobalRotation = Facing(player, mobs)
                                    - SwingAngle / 2.0f
                                    + Mathf.Tau * spawnIndex / Mathf.Max(1, spawnTotal);
    }

    public override void Travel(Projectile projectile, double delta)
    {
        if (projectile.Player != null && IsInstanceValid(projectile.Player)) projectile.GlobalPosition = projectile.Player.GlobalPosition;
        if (SwingAngle == 0.0f || projectile.Data == null) return;
        if (projectile.Data.Lifetime > 0.0f)
            projectile.GlobalRotation += SwingAngle * (float)delta / projectile.Data.Lifetime;
        else
            projectile.GlobalRotation += SwingAngle * (float)delta;
    }

    private float Facing(Player player, Array<Node> mobs)
    {
        Mob best = null;
        var bestDistance = Mathf.Inf;
        foreach (var mob in mobs)
        {
            if (mob is not Mob m) continue;
            var distance = player.GlobalPosition.DistanceSquaredTo(m.GlobalPosition);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = m;
            }
        }

        if (best == null) return 0.0f;
        return (best.GlobalPosition - player.GlobalPosition).Angle();
    }
}