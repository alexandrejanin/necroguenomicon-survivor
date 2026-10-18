using Godot;

[GlobalClass]
public partial class Effect : Resource
{
	// Triggered with the anchor position (hit/death/expire point).
	// HitMob is null for expiry. Killed is true only on the kill pass.
	public virtual void Trigger(Vector2 anchor, Player player,
		Godot.Collections.Array<Node> mobs, Mob hitMob, bool killed)
	{
	}
}
