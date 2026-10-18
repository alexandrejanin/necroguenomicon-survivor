using Godot;
using Godot.Collections;

[GlobalClass]
public partial class FiringPattern : Resource
{
    // How many spawn + where (absolute positions, relative to the anchor:
    // the player for casts, the trigger position for effects).
    public virtual Array<Vector2> GetSpawns(
        Vector2 anchor, Player player, int castIndex,
        Array<Node> mobs, Vector2 hit)
    {
        return [anchor];
    }

    public virtual Array<Stat> GetStats()
    {
        return [];
    }

    public virtual int DesiredCount()
    {
        return 1;
    }
}