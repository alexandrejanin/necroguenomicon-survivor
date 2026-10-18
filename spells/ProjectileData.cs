using Godot;
using Godot.Collections;

[GlobalClass]
public partial class ProjectileData : Resource
{
    [Export] public float HitCooldown;

    [Export] public float Lifetime = 4.0f;

    // Pure data for one projectile type. Tweak in the editor.
    [Export] public Stat Damage { get; set; }
    [Export] public Stat Pierce { get; set; }
    [Export] public Movement Movement { get; set; }
    [Export] public PackedScene Visual { get; set; }
    [Export] public Array<Effect> OnHitEffects { get; set; } = [];
    [Export] public Array<Effect> OnKillEffects { get; set; } = [];
    [Export] public Array<Effect> OnExpireEffects { get; set; } = [];

    public Array<Stat> GetStats()
    {
        var stats = new Array<Stat> { Damage, Pierce };
        if (Movement != null)
            foreach (var s in Movement.GetStats())
                stats.Add(s);

        var filtered = new Array<Stat>();
        foreach (var s in stats)
            if (s != null)
                filtered.Add(s);

        return filtered;
    }
}