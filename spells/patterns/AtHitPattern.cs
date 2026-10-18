using Godot;
using Godot.Collections;

[GlobalClass]
public partial class AtHitPattern : FiringPattern
{
    // Spawns at the trigger position (Piercing dagger). For effects the
    // anchor already is the hit/death/expire point, so it is returned as-is.
    public override Array<Vector2> GetSpawns(
        Vector2 anchor, Player player, int castIndex,
        Array<Node> mobs, Vector2 hit)
    {
        return [hit != Vector2.Zero ? hit : anchor];
    }
}