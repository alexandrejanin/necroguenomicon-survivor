class_name Fireball
extends Projectile

@export var SPEED = 70.0

var explosion = preload("res://scenes/spells/explosion.tscn")

var player_: Player
var rb_: RigidBody2D

func on_shoot(player: Player) -> bool:
	player_ = player
	rb_ = $RigidBody2D
	
	if rb_ == null:
		printerr("rb == null")
		return false
	
	global_position = player.global_position
	
	var nearest_mob: Node2D = null
	for mob: Node2D in player.get_node("%MobSpawner").get_children():
		if nearest_mob == null \
		   || global_position.distance_squared_to(mob.global_position) \
			< global_position.distance_squared_to(nearest_mob.global_position):
			nearest_mob = mob
			
	if nearest_mob == null:
		return false
	
	var direction := global_position.direction_to(nearest_mob.global_position)
	
	rotation = direction.angle()
	
	rb_.contact_monitor = true
	rb_.max_contacts_reported = 1
	rb_.body_entered.connect(on_collision)
	
	rb_.linear_velocity = SPEED * direction
	rb_.freeze = false
	
	return true

func on_collision(other: Node2D) -> void:
	var impact_center = (rb_.global_position + other.global_position) / 2.0
	var explosion_instance: Node2D = explosion.instantiate()
	explosion_instance.global_position = impact_center
	add_sibling.call_deferred(explosion_instance)
	queue_free()
