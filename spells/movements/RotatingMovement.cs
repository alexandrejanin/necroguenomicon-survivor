using Godot;
using Godot.Collections;

[GlobalClass]
public partial class RotatingMovement : Movement
{
    [Export] public float AngleStep = Mathf.Pi / 4.0f;
    [Export] public float Speed = 100.0f;

    public override void Setup(Projectile projectile, Player player, Vector2 spawnPos,
        int castIndex, int spawnIndex, int spawnTotal,
        Array<Node> mobs, Vector2 hit)
    {
        base.Setup(projectile, player, spawnPos, castIndex, spawnIndex, spawnTotal, mobs, hit);
        var baseAngle = Mathf.Pi
                        + AngleStep * castIndex
                        + Mathf.Tau * spawnIndex / Mathf.Max(1, spawnTotal);
        projectile.Rotation = baseAngle;
        projectile.LinearVelocity = Vector2.Right.Rotated(baseAngle) * Speed;
    }
}