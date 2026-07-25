@abstract
class_name Projectile
extends RigidBody2D

var _player: Player = null

@abstract
func _on_shoot(player: Player, index: int) -> bool

@abstract
func on_hit(mob: Mob) -> void

func on_shoot(player: Player, index: int) -> bool:
	_player = player
	global_position = player.global_position
	return _on_shoot(player, index)

func shoot_towards(point: Vector2, speed: float) -> void:
	var direction := global_position.direction_to(point)
	rotation = direction.angle()
	linear_velocity = speed * direction

func get_nearest_mob() -> Node2D:
	var nearest_mob: Node2D = null
	for mob: Node2D in _player.get_node("%MobSpawner").get_children():
		if nearest_mob == null \
		   || global_position.distance_squared_to(mob.global_position) \
			< global_position.distance_squared_to(nearest_mob.global_position):
			nearest_mob = mob
	return nearest_mob
	
