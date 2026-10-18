using Godot;

[GlobalClass]
public partial class PlayerInfo : Label
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
			return;
		}
		Text = $"Level {_player.Level}";
	}
}
