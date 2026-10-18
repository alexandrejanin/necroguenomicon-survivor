using Godot;

[GlobalClass]
public partial class XpBar : ColorRect
{
    private Player _player;

    public override void _Ready()
    {
        _player = GetNode<Player>("/root/Game/Player");
    }

    public override void _Process(double delta)
    {
        if (_player == null)
        {
            GD.Print("player == null");
            return;
        }

        var scale = Scale;
        scale.X = _player.Xp / Mathf.Max(_player.XpThreshold(), 1.0f);
        Scale = scale;
    }
}