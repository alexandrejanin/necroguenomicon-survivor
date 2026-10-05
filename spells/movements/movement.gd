class_name Movement
extends Resource

# Initial direction + travel. setup() places/orients the projectile,
# travel() runs every physics frame.
func setup(projectile: Projectile, _player: Player, spawn_pos: Vector2, _cast_index: int, _spawn_index: int, _spawn_total: int, _mobs: Array, _hit: Vector2) -> void:
	projectile.global_position = spawn_pos


func travel(_projectile: Projectile, _delta: float) -> void:
	pass


func get_stats() -> Array:
	return []
