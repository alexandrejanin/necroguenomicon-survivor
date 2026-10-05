class_name CountPattern
extends FiringPattern

@export var count: Stat


func get_spawns(anchor: Vector2, player: Player, cast_index: int, mobs: Array, hit: Vector2) -> Array[Vector2]:
	var total := 1
	if count != null:
		total = maxi(1, int(count.get_value()))
	var spawns: Array[Vector2] = []
	for i in total:
		spawns.append(anchor)
	return spawns


func get_stats() -> Array:
	if count == null:
		return []
	return [count]


func desired_count() -> int:
	if count == null:
		return 1
	return maxi(1, int(count.get_value()))
