using Godot;
using Godot.Collections;

[GlobalClass]
public partial class CountPattern : FiringPattern
{
    [Export] public Stat Count;

    public override Array<Stat> GetStats()
    {
        return [Count];
    }

    public override Array<Vector2> GetSpawns(
        Vector2 anchor, Player player, int castIndex,
        Array<Node> mobs, Vector2 hit)
    {
        var total = 1;
        if (Count != null) total = Mathf.Max(1, (int)Count.GetValue());
        var spawns = new Array<Vector2>();
        for (var i = 0; i < total; i++) spawns.Add(anchor);
        return spawns;
    }

    public override int DesiredCount()
    {
        return Mathf.Max(1, (int)(Count?.GetValue() ?? 1));
    }
}