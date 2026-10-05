class_name RotatingMovement
extends Movement

@export var angle_step := PI / 4.0
@export var speed := 100.0


func setup(projectile: Projectile, player: Player, spawn_pos: Vector2, cast_index: int, spawn_index: int, spawn_total: int, mobs: Array, hit: Vector2) -> void:
	super.setup(projectile, player, spawn_pos, cast_index, spawn_index, spawn_total, mobs, hit)
	var base_angle := PI + angle_step * float(cast_index) + TAU * float(spawn_index) / float(maxi(1, spawn_total))
	projectile.rotation = base_angle
	projectile.linear_velocity = Vector2.RIGHT.rotated(base_angle) * speed


func get_stats() -> Array:
	return []
