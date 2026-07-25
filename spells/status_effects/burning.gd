class_name Burning
extends StatusEffect

@export var DURATION := 5.0
@export var DPS := 10.0

var timer := 0.0
var damage_counter := 0.0

func tick(delta: float, mob: Mob) -> bool:
	damage_counter += delta * DPS
	while damage_counter >= 1.0:
		mob.take_damage(1)
		damage_counter -= 1
	
	timer += delta
	return timer >= DURATION
