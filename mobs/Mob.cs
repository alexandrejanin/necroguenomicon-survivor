using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Mob : RigidBody2D
{
    private static readonly PackedScene XpPickupScene = GD.Load<PackedScene>("res://pickups/xp.tscn");
    private static readonly PackedScene HeartPickupScene = GD.Load<PackedScene>("res://pickups/heart.tscn");

    public int Hp;
    [Export] public int MaxHp = 100;
    [Export] public float Speed = 10.0f;

    private Area2D _hitbox;
    private Area2D _hurtbox;
    private Player _player;
    private Sprite2D _sprite;
    private Array<StatusEffect> StatusEffects { get; } = [];

    public override void _Ready()
    {
        Hp = MaxHp;
        _player = GetNode<Player>("/root/Game/Player");
        _sprite = GetNode<Sprite2D>("Sprite");
        _hitbox = GetNode<Area2D>("Hitbox");
        _hurtbox = GetNode<Area2D>("Hurtbox");
        _hitbox.BodyEntered += OnHit;
        _hurtbox.BodyEntered += OnHurt;
    }

    public override void _Process(double delta)
    {
        // Refactor: GDScript discarded Array.filter()'s return value, so expired
        // effects were never removed. Remove finished effects (Tick returns true).
        for (var i = StatusEffects.Count - 1; i >= 0; i--)
            if (StatusEffects[i].Tick(delta, this))
                StatusEffects.RemoveAt(i);

        _sprite.Modulate = new Color(2.0f, 0.5f, 0.5f)
            .Lerp(new Color(1.0f, 1.0f, 1.0f), Mathf.Sqrt((float)Hp / MaxHp));
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        if (_player == null) return;
        state.LinearVelocity = Speed * GlobalPosition.DirectionTo(_player.GlobalPosition);
        _sprite.FlipH = state.LinearVelocity.X > 0.0f;
    }

    public override void _ExitTree()
    {
        var pickupScene = GD.RandRange(0, 99) == 0 ? HeartPickupScene : XpPickupScene;
        var pickup = pickupScene.Instantiate<Node2D>();
        pickup.GlobalPosition = GlobalPosition;
        GetNode("/root/Game").CallDeferred(Node.MethodName.AddChild, pickup);
        GetNode<RunManager>("/root/Game/RunManager").TotalKills += 1;
    }

    private void OnHit(Node other)
    {
        if (other == _player) _player.TakeDamage();
    }

    private void OnHurt(Node other)
    {
        if (other is Projectile projectile) projectile.OnHit(this);
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0) QueueFree();
    }
}