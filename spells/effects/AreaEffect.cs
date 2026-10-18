using Godot;
using Godot.Collections;

[GlobalClass]
public partial class AreaEffect : Effect
{
    [Export] public float Duration = 0.3f;

    // Damages all mobs within radius of the anchor (Crystal explosion,
    // Glacier burst, Lightning strike core, Poison cloud flash).
    // The visual is cosmetic only and may be null.
    [Export] public Stat Damage { get; set; }
    [Export] public Stat Radius { get; set; }
    [Export] public PackedScene Visual { get; set; }

    public override void Trigger(Vector2 anchor, Player player,
        Array<Node> mobs, Mob hitMob, bool killed)
    {
        var damage = Damage?.GetValue() ?? 0.0f;
        var radius = Radius?.GetValue() ?? 40.0f;
        if (damage > 0.0f)
            foreach (var mob in mobs)
            {
                if (mob is not Mob m || !IsInstanceValid(m)) continue;
                if (m.GlobalPosition.DistanceTo(anchor) <= radius) m.TakeDamage((int)damage);
            }

        if (Visual != null && player != null && IsInstanceValid(player))
        {
            var flash = Visual.Instantiate<Node2D>();
            if (flash == null) return;
            player.AddSibling(flash);
            flash.GlobalPosition = anchor;
            var timer = player.GetTree().CreateTimer(Mathf.Max(0.01f, Duration));
            timer.Timeout += () =>
            {
                if (IsInstanceValid(flash)) flash.QueueFree();
            };
        }
    }
}