using Godot;

[GlobalClass]
public abstract partial class StatusEffect : Resource
{
	public abstract bool Tick(double delta, Mob mob);
}
