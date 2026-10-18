using Godot;
using Godot.Collections;

[GlobalClass]
public partial class OrbitMovement : Movement
{
    // Permanent ring around the player (Orb). Slot angle snaps on setup
    // so desired_count() top-ups fill the missing slots.
    [Export] public Stat Radius { get; set; }
    [Export] public Stat AngularSpeed { get; set; }

    public override Array<Stat> GetStats()
    {
        return [Radius, AngularSpeed];
    }

    public override void Setup(Projectile projectile, Player player, Vector2 spawnPos,
        int castIndex, int spawnIndex, int spawnTotal,
        Array<Node> mobs, Vector2 hit)
    {
        projectile.LinearVelocity = Vector2.Zero;
        var angle = Mathf.Tau * spawnIndex / Mathf.Max(1, spawnTotal);
        projectile.GlobalPosition = player.GlobalPosition + Vector2.Right.Rotated(angle) * GetRadius();
    }

    public override void Travel(Projectile projectile, double delta)
    {
        if (projectile.Player == null || !IsInstanceValid(projectile.Player))
            return;

        var offset = projectile.GlobalPosition - projectile.Player.GlobalPosition;
        var angle = offset.Angle();

        if (offset.LengthSquared() < 0.001f)
            angle = 0.0f;

        angle += GetAngularSpeed() * (float)delta;
        projectile.GlobalPosition = projectile.Player.GlobalPosition + Vector2.Right.Rotated(angle) * GetRadius();
    }

    private float GetRadius()
    {
        return Radius?.GetValue() ?? 42.0f;
    }

    private float GetAngularSpeed()
    {
        return AngularSpeed?.GetValue() ?? 2.6f;
    }
}