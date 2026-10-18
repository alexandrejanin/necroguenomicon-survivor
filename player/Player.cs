using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Player : CharacterBody2D
{
    public int Hp;
    [Export] public float InvincibilityDuration = 3.0f;
    public int Level = 0;
    [Export] public int MaxHp = 3;
    [Export] public float PickupRange = 27.0f;
    [Export] public float Speed = 30.0f;
    public int Xp = 0;

    private double _invincibilityTimer;
    private Sprite2D _sprite;
    public Array<Spell> Spells { get; } = [];

    public override void _Ready()
    {
        Hp = MaxHp;
        _invincibilityTimer = InvincibilityDuration;
        _sprite = GetNode<Sprite2D>("Sprite");
    }

    public override void _Process(double delta)
    {
        _invincibilityTimer += delta;

        _sprite.Visible = Hp > 0
                          && (_invincibilityTimer > InvincibilityDuration
                              || (int)(_invincibilityTimer / 0.1) % 2 != 0);

        if (Hp <= 0) return;

        foreach (var spell in Spells) spell.Tick(this, delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Hp <= 0) return;

        var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Velocity = Speed * direction;
        MoveAndSlide();

        if (Velocity.X < 0.0f)
            _sprite.FlipH = true;
        else if (Velocity.X > 0.0f) _sprite.FlipH = false;
    }

    public int XpThreshold()
    {
        return (int)(10.0f * Mathf.Sqrt(Level));
    }

    public void TakeDamage(int damage = 1)
    {
        if (Hp <= 0 || _invincibilityTimer < InvincibilityDuration) return;
        Hp -= damage;
        _invincibilityTimer = 0.0;
    }
}