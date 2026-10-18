using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Projectile : RigidBody2D
{
    private readonly System.Collections.Generic.Dictionary<ulong, ulong> _lastHit = new();
    public ProjectileData Data;

    // Generic runner: built from a Spell's ProjectileData, behaves per Movement,
    // fires Effect lists on hit/kill/expire. No per-weapon code lives here.
    public Player Player;
    public int Slot;
    public Spell Spell;

    private double _age;
    private bool _expired;
    private bool _pierceInfinite;
    private int _pierceLeft;

    public void Setup(Player newPlayer, Spell newSpell, Vector2 spawnPos,
        int castIndex, int spawnIndex, int spawnTotal,
        Array<Node> mobs, Vector2 hit)
    {
        Player = newPlayer;
        Spell = newSpell;
        Data = Spell.Projectile;
        Slot = spawnIndex;
        var maxPierce = 0;
        if (Data.Pierce != null) maxPierce = (int)Data.Pierce.GetValue();
        _pierceInfinite = maxPierce < 0;
        _pierceLeft = maxPierce;
        Data.Movement.Setup(this, Player, spawnPos, castIndex, spawnIndex, spawnTotal, mobs, hit);
    }

    public override void _Process(double delta)
    {
        if (Data == null) return;
        _age += delta;
        // lifetime <= 0 = infinite (Orb).
        if (Data.Lifetime > 0.0f && _age >= Data.Lifetime)
        {
            Expire(GlobalPosition);
            return;
        }

        Data.Movement.Travel(this, delta);
    }

    public void OnHit(Mob mob)
    {
        if (Data == null || mob == null || !IsInstanceValid(mob))
            return;

        var cooldown = Data.HitCooldown;
        var id = mob.GetInstanceId();
        var now = Time.GetTicksMsec();

        if (_lastHit.TryGetValue(id, out var prev) && (now - prev) / 1000.0f < cooldown)
            return;

        _lastHit[id] = now;
        mob.TakeDamage(Damage());
        var anchor = mob.GlobalPosition;

        foreach (var effect in Data.OnHitEffects)
            effect.Trigger(anchor, Player, Mobs(), mob, false);

        if (mob.Hp <= 0)
            foreach (var effect in Data.OnKillEffects)
                effect.Trigger(anchor, Player, Mobs(), mob, true);

        if (_pierceInfinite)
            return;

        _pierceLeft--;
        if (_pierceLeft < 0)
            Expire(anchor);
    }

    private int Damage()
    {
        if (Data.Damage == null) return 1;
        return (int)Data.Damage.GetValue();
    }

    // Movement may request expiry too (Boomerang returning home). Idempotent
    // so on_expire_effects fire at most once per projectile.
    public void Expire(Vector2 anchor)
    {
        if (_expired) return;
        _expired = true;
        foreach (var effect in Data.OnExpireEffects) effect.Trigger(anchor, Player, Mobs(), null, false);
        QueueFree();
    }

    private Array<Node> Mobs()
    {
        if (Player == null) return new Array<Node>();
        return Player.GetNode("%MobSpawner").GetChildren();
    }
}