using System.Collections.Generic;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Spell : Resource
{
    public double CooldownTimer;
    [Export] public string Name = "Spell";
    public int ProjectileCounter;
    [Export] public Texture2D Icon { get; set; }
    [Export] public Stat FireRate { get; set; }
    [Export] public FiringPattern Pattern { get; set; }
    [Export] public ProjectileData Projectile { get; set; }

    public void Tick(Player player, double delta)
    {
        // No FireRate = PERMANENT: keep the target alive instead of casting.
        if (FireRate == null)
        {
            TopUp(player);
            return;
        }

        CooldownTimer += delta;
        if (CooldownTimer < 1.0 / FireRate.GetValue()) return;
        Cast(player, player.GlobalPosition, Vector2.Zero);
        CooldownTimer = 0.0;
        ProjectileCounter++;
    }

    public void OnAcquired(Player player)
    {
        // Nothing to do: the next tick tops up PERMANENT spells,
        // TIMED spells start casting on cooldown.
    }

    // PERMANENT upkeep: spawn whichever desired slots have no live projectile.
    // Covers first spawn, replacement, and count upgrades with one rule.
    private void TopUp(Player player)
    {
        if (Pattern == null || Projectile == null || Projectile.Visual == null) return;
        if (player.GetParent() == null) return;
        var desired = Pattern.DesiredCount();
        if (desired <= 0) return;
        var live = new HashSet<int>();
        foreach (var sibling in player.GetParent().GetChildren())
            if (sibling is Projectile p && p.Spell == this)
                live.Add(p.Slot);

        if (live.Count >= desired) return;
        var mobs = player.GetNode("%MobSpawner").GetChildren();
        for (var i = 0; i < desired; i++)
            if (!live.Contains(i))
                Spawn(player, player.GlobalPosition, Vector2.Zero, i, desired, mobs);
    }

    public void Cast(Player player, Vector2 anchor, Vector2 hit)
    {
        if (Pattern == null || Projectile == null || Projectile.Visual == null) return;
        var mobs = player.GetNode("%MobSpawner").GetChildren();
        var spawns = Pattern.GetSpawns(anchor, player, ProjectileCounter, mobs, hit);
        var total = spawns.Count;
        for (var i = 0; i < total; i++) Spawn(player, spawns[i], hit, i, total, mobs);
    }

    private void Spawn(Player player, Vector2 spawnPos, Vector2 hit,
        int spawnIndex, int spawnTotal, Array<Node> mobs)
    {
        var instance = Projectile.Visual.Instantiate<Projectile>();
        if (instance == null) return;
        player.AddSibling(instance);
        instance.Setup(player, this, spawnPos, ProjectileCounter, spawnIndex, spawnTotal, mobs, hit);
    }

    public Array<Stat> GetStats()
    {
        var stats = new Array<Stat>();
        if (FireRate != null && FireRate.PerLevel > 0.0f) stats.Add(FireRate);
        if (Pattern != null)
            foreach (var stat in Pattern.GetStats())
                if (stat.PerLevel > 0.0f)
                    stats.Add(stat);

        if (Projectile != null)
            foreach (var stat in Projectile.GetStats())
                if (stat.PerLevel > 0.0f)
                    stats.Add(stat);

        return stats;
    }
}