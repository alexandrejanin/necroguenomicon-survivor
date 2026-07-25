class_name Dagger
extends Projectile

@export var SPEED = 100.0

func _on_shoot(player: Player, index: int) -> bool:
	shoot_towards(global_position + Vector2.LEFT.rotated(PI/2 * index), SPEED)
	return true

func on_hit(mob: Mob) -> void:
	mob.queue_free()
	queue_free()
