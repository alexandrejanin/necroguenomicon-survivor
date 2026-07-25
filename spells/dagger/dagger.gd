class_name Dagger
extends Projectile

@export var SPEED = 100.0
@export var DAMAGE := 100

func _on_shoot(index: int) -> bool:
	shoot_towards(global_position + Vector2.LEFT.rotated(PI/4 * index), SPEED)
	return true

func on_hit(mob: Mob) -> void:
	mob.take_damage(DAMAGE)
	queue_free()
