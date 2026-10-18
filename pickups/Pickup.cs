using Godot;

[GlobalClass]
public abstract partial class Pickup : Sprite2D
{
    private static readonly Curve AnimationCurve = GD.Load<Curve>("res://pickups/animation_curve.tres");
    [Export] public float AnimationDuration = 0.5f;

    protected Player Player;

    private double _animationTimer;
    private bool _inRange;
    private Vector2 _startingPosition;

    public override void _Ready()
    {
        Player = GetNode<Player>("/root/Game/Player");
    }

    protected abstract void OnPickup();

    public override void _Process(double delta)
    {
        if (Player == null)
        {
            GD.PrintErr("player == null");
            return;
        }

        if (!_inRange && GlobalPosition.DistanceSquaredTo(Player.GlobalPosition) < Player.PickupRange * Player.PickupRange)
        {
            _inRange = true;
            _startingPosition = GlobalPosition;
        }

        if (_inRange)
        {
            _animationTimer += delta;
            var t = (float)(_animationTimer / AnimationDuration);
            GlobalPosition = _startingPosition.Lerp(Player.GlobalPosition, AnimationCurve.Sample(t));
            var whiteness = Mathf.Max(10.0f * t, 1.0f);
            Modulate = new Color(whiteness, whiteness, whiteness);

            if (t >= 1.0f)
            {
                OnPickup();
                QueueFree();
            }
        }
    }
}