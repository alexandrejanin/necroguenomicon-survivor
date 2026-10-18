using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Movement : Resource
{
    public virtual Array<Stat> GetStats()
    {
        return [];
    }

    // Initial direction + travel. Setup() places/orients the projectile,
    // Travel() runs every frame.
    public virtual void Setup(Projectile projectile, Player player, Vector2 spawnPos,
        int castIndex, int spawnIndex, int spawnTotal,
        Array<Node> mobs, Vector2 hit)
    {
        projectile.GlobalPosition = spawnPos;
    }

    public virtual void Travel(Projectile projectile, double delta)
    {
    }
}