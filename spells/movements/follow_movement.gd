class_name FollowMovement
extends Movement

# Glued to the player; faces nearest mob once on spawn, then swings.
# Forward reach lives in the visual scene (sprite/shape offset).
@export var swing_angle := 0.0


func setup(projectile: Projectile, player: Player, spawn_pos: Vector2, cast_index: int, spawn_index: int, spawn_total: int, mobs: Array, hit: Vector2) -> void:
	super.setup(projectile, player, spawn_pos, cast_index, spawn_index, spawn_total, mobs, hit)
	projectile.global_rotation = _facing(player, mobs) - swing_angle / 2.0 + TAU * float(spawn_index) / float(maxi(1, spawn_total))


func travel(projectile: Projectile, delta: float) -> void:
	if projectile.player != null and is_instance_valid(projectile.player):
		projectile.global_position = projectile.player.global_position
	if swing_angle == 0.0 or projectile.data == null:
		return
	if projectile.data.lifetime > 0.0:
		projectile.global_rotation += swing_angle * delta / projectile.data.lifetime
	else:
		projectile.global_rotation += swing_angle * delta


func _facing(player: Player, mobs: Array) -> float:
	var best: Mob = null
	var best_distance := INF
	for mob in mobs:
		if not (mob is Mob):
			continue
		var distance := player.global_position.distance_squared_to(mob.global_position)
		if distance < best_distance:
			best_distance = distance
			best = mob
	if best == null:
		return 0.0
	return (best.global_position - player.global_position).angle()
