class_name Lightning
extends Projectile

@export var DAMAGE := 40
@export var BOUNCES := 2
@export var BOUNCE_RANGE := 32.0
@export var DURATION := 0.15

var timer := 0.0

func _process(delta: float) -> void:
	timer += delta
	if timer >= DURATION:
		queue_free()

func _on_shoot(_index: int) -> bool:
	var mobs = get_mob_chain()
	
	if mobs.is_empty():
		return false
	
	var starting_position := global_position
	for mob in mobs:
		var delta = mob.global_position - starting_position
		var count = int(delta.length() / 8)
		for i in range(count):
			var new_lightning = self.duplicate()
			new_lightning.global_position = mob.global_position.lerp(starting_position, float(i)/count)
			new_lightning.global_rotation = delta.angle() + randf_range(-PI/8,PI/8)
			add_sibling(new_lightning)
		starting_position = mob.global_position
		mob.take_damage(DAMAGE)
	
	return true

func on_hit(mob: Mob) -> void:
	mob.take_damage(DAMAGE)

func get_mob_chain() -> Array[Mob]:
	var all_mobs = get_all_mobs()
	
	if all_mobs.is_empty():
		return []
	
	var mob_chain: Array[Mob] = []
	var starting_position = global_position
	for i in range(BOUNCES + 1):
		var best_mob: Mob = null
		for mob in all_mobs:
			if mob_chain.has(mob):
				continue
			var distance = mob.global_position.distance_squared_to(starting_position)
			if distance > (2 if i == 0 else 1) * BOUNCE_RANGE*BOUNCE_RANGE:
				continue
			if best_mob != null && best_mob.global_position.distance_squared_to(starting_position) < distance:
				continue
			best_mob = mob
		if best_mob == null:
			return mob_chain
		mob_chain.append(best_mob)
		starting_position = best_mob.global_position
	
	return mob_chain
	
	
