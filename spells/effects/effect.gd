class_name Effect
extends Resource

# Triggered with the anchor position (hit/death/expire point).
# hit_mob is null for expiry. killed is true only on the kill pass.
func trigger(_anchor: Vector2, _player: Player, _mobs: Array, _hit_mob: Mob, _killed: bool) -> void:
	pass
