class_name Explosion
extends Projectile

@export var DURATION := 0.25
@export var DAMAGE := 50

var burning := preload("res://spells/status_effects/burning.tres")

var timer := 0.0

func _ready() -> void:
	$AnimatedSprite2D.play()
	
func _process(delta: float) -> void:
	timer += delta
	if timer >= DURATION:
		queue_free()
	
func _on_shoot(index: int) -> bool:
	return true

func on_hit(mob: Mob) -> void:
	mob.take_damage(DAMAGE)
	mob.status_effects.append(burning.duplicate())
