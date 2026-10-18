using Godot;

[GlobalClass]
public partial class Burning : StatusEffect
{
	[Export] public float Duration = 5.0f;
	[Export] public float Dps = 10.0f;

	private double _timer = 0.0;
	private double _damageCounter = 0.0;

	public override bool Tick(double delta, Mob mob)
	{
		_damageCounter += delta * Dps;
		while (_damageCounter >= 1.0)
		{
			mob.TakeDamage(1);
			_damageCounter -= 1.0;
		}
		_timer += delta;
		return _timer >= Duration;
	}
}
