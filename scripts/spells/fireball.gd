class_name Fireball
extends Projectile

@export var SPEED = 70.0

var explosion = preload("res://scenes/spells/explosion.tscn")

func _on_shoot(player: Player, index: int) -> bool:
	var nearest_mob = get_nearest_mob()
	if nearest_mob == null:
		return false
		
	shoot_towards(nearest_mob.global_position, SPEED)
	return true

func on_hit(mob: Mob) -> void:
	var explosion_instance: Node2D = explosion.instantiate()
	explosion_instance.global_position = mob.global_position
	add_sibling.call_deferred(explosion_instance)
	queue_free()
