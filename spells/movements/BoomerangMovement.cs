using Godot;
using Godot.Collections;

[GlobalClass]
public partial class BoomerangMovement : Movement
{
    private readonly System.Collections.Generic.Dictionary<ulong, bool> _returning = new();
    private readonly System.Collections.Generic.Dictionary<ulong, Node2D> _visuals = new();
    [Export] public float CatchRadius = 10.0f;
    [Export] public Stat OutwardRange;

    // Aims a random mob on setup, flies straight out to outward_range, then
    // homes and frees once within catch_radius of the player.
    //
    // Engine constraint: Travel must never write the body's transform.
    // Setting Rotation every frame on this rotation-locked RigidBody2D
    // suppresses physics integration (verified: velocity stays ~140 but
    // the body advances ~v/60 per second). Steering is velocity-only and
    // the facing is applied to the Sprite2D child, which is pure visual.
    [Export] public float Speed = 140.0f;

    public override Array<Stat> GetStats()
    {
        return [OutwardRange];
    }

    public override void Setup(Projectile projectile, Player player, Vector2 spawnPos,
        int castIndex, int spawnIndex, int spawnTotal,
        Array<Node> mobs, Vector2 hit)
    {
        base.Setup(projectile, player, spawnPos, castIndex, spawnIndex, spawnTotal, mobs, hit);
        var dir = Facing(player, spawnPos, mobs);
        projectile.Rotation = dir.Angle();
        projectile.LinearVelocity = dir * Speed;
        var id = projectile.GetInstanceId();
        _returning[id] = false;
        _visuals[id] = projectile.GetNodeOrNull<Node2D>("Sprite2D");
        projectile.TreeExiting += () =>
        {
            _returning.Remove(id);
            _visuals.Remove(id);
        };
    }

    public override void Travel(Projectile projectile, double delta)
    {
        if (projectile.Player == null || !IsInstanceValid(projectile.Player))
            return;

        var toPlayer = projectile.Player.GlobalPosition - projectile.GlobalPosition;
        var distance = toPlayer.Length();
        if (distance < 0.001f)
            return;

        var id = projectile.GetInstanceId();

        // Per-projectile phase. The outbound leg must finish at outward_range
        // before homing begins. Reading the instantaneous velocity direction
        // instead misreads the frame after launch: the boomerang spawns on top
        // of the player, so a moving player puts them slightly behind it and
        // flips the sign while the boomerang is still within catch_radius.
        _returning.TryGetValue(id, out var returning);
        if (!returning)
        {
            var range = OutwardRange?.GetValue() ?? 160.0f;
            if (distance < range || distance <= CatchRadius)
            {
                // Fly straight on the way out; re-apply the launch heading if
                // the body's velocity was somehow cleared.
                if (projectile.LinearVelocity.LengthSquared() < 0.001f)
                    projectile.LinearVelocity = Vector2.Right.Rotated(projectile.Rotation) * Speed;
                FaceVelocity(projectile, projectile.LinearVelocity);
                return;
            }

            _returning[id] = true;
        }

        // Home again; free on the frame it reaches the player, firing
        // on_expire_effects like any other expiry.
        if (distance <= CatchRadius)
        {
            projectile.Expire(projectile.GlobalPosition);
            return;
        }

        var velocity = toPlayer.Normalized() * Speed;
        projectile.LinearVelocity = velocity;
        FaceVelocity(projectile, velocity);
    }

    private void FaceVelocity(Projectile projectile, Vector2 velocity)
    {
        if (velocity.LengthSquared() < 0.001f) return;
        var id = projectile.GetInstanceId();
        if (!_visuals.TryGetValue(id, out var visual) || !IsInstanceValid(visual))
        {
            visual = projectile.GetNodeOrNull<Node2D>("Sprite2D");
            if (visual == null)
            {
                _visuals.Remove(id);
                return;
            }

            _visuals[id] = visual;
        }

        visual.Rotation = velocity.Angle() - projectile.Rotation;
    }

    private Vector2 Facing(Player player, Vector2 spawnPos, Array<Node> mobs)
    {
        Mob pick = null;
        var count = 0;
        foreach (var mob in mobs)
        {
            if (mob is not Mob m || !IsInstanceValid(m)) continue;
            count++;
            if (GD.RandRange(0, count - 1) == 0) pick = m;
        }

        if (pick != null)
        {
            var toMob = pick.GlobalPosition - spawnPos;
            if (toMob.LengthSquared() > 0.001f) return toMob.Normalized();
        }

        return Vector2.Right.Rotated((float)GD.RandRange(0.0f, Mathf.Tau));
    }
}
