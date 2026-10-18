using Godot;

[GlobalClass]
public partial class RunManager : Node
{
	public int TotalXp = 0;
	public int TotalKills = 0;

	public string GetText()
	{
		return $"Killed {TotalKills} enemies\nPicked up {TotalXp} XP";
	}
}
