class_name FiringPattern
extends Resource

# How many spawn + where (absolute positions, relative to the anchor:
# the player for casts, the trigger position for effects).
func get_spawns(anchor: Vector2, _player: Player, _cast_index: int, _mobs: Array, _hit: Vector2) -> Array[Vector2]:
	var spawns: Array[Vector2] = [anchor]
	return spawns


func get_stats() -> Array:
	return []


func desired_count() -> int:
	return 1
