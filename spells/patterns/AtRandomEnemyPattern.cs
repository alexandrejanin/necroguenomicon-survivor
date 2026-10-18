using Godot;
using Godot.Collections;

[GlobalClass]
public partial class AtRandomEnemyPattern : FiringPattern
{
    // Spawns next to a random enemy (Tentacle buddy). Places away from the
    // anchor; the facing is left to the scene / StationaryMovement.
    [Export] public Vector2 Offset = Vector2.Zero;

    public override Array<Vector2> GetSpawns(
        Vector2 anchor, Player player, int castIndex,
        Array<Node> mobs, Vector2 hit)
    {
        Mob pick = null;
        var count = 0;
        foreach (var mob in mobs)
        {
            if (mob is not Mob m || !IsInstanceValid(m)) continue;
            count++;
            if (GD.RandRange(0, count - 1) == 0) pick = m;
        }

        if (pick == null) return new Array<Vector2> { anchor };
        return new Array<Vector2> { pick.GlobalPosition + Offset };
    }
}