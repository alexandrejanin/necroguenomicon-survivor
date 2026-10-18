using Godot;

[GlobalClass]
public partial class DefeatScreen : Label
{
	private Player _player;

	public override void _Ready()
	{
		_player = GetNode<Player>("/root/Game/Player");
	}

	public override void _Process(double delta)
	{
		Visible = _player.Hp <= 0;

		if (Visible)
		{
			Text = $"{GetNode<RunManager>("/root/Game/RunManager").GetText()}\nPress 'R' to try again!";
			if (Input.IsKeyPressed(Key.R))
			{
				GetTree().ReloadCurrentScene();
			}
		}
	}
}
