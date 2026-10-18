using Godot;

[GlobalClass]
public partial class HpBar : TextureRect
{
    [Export] public float WidthPerHp = 16.0f;

    private Player _player;

    public override void _Ready()
    {
        _player = GetNode<Player>("/root/Game/Player");
    }

    public override void _Process(double delta)
    {
        if (_player == null)
        {
            GD.PrintErr("player == null");
            return;
        }

        var size = Size;
        size.X = WidthPerHp * _player.Hp;
        Size = size;
        Visible = _player.Hp > 0;
    }
}