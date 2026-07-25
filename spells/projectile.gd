@abstract
class_name Projectile
extends RigidBody2D

var player: Player = null

@abstract
func _on_shoot(index: int) -> bool

@abstract
func on_hit(mob: Mob) -> void

func on_shoot(new_player: Player, index: int) -> bool:
	player = new_player
	global_position = player.global_position
	return _on_shoot(index)

func shoot_towards(point: Vector2, speed: float) -> void:
	var direction := global_position.direction_to(point)
	rotation = direction.angle()
	linear_velocity = speed * direction

func get_all_mobs() -> Array[Mob]:
	return player.get_node("%MobSpawner").get_children() as Array[Mob]

func get_nearest_mob() -> Node2D:
	var nearest_mob: Node2D = null
	for mob in get_all_mobs():
		if nearest_mob == null \
		   || global_position.distance_squared_to(mob.global_position) \
			< global_position.distance_squared_to(nearest_mob.global_position):
			nearest_mob = mob
	return nearest_mob
