class_name Lightning
extends Projectile

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
		mob.take_damage(spell.stats.get("DAMAGE").get_value())
	
	return true

func on_hit(_mob: Mob) -> void:
	pass

func get_mob_chain() -> Array[Mob]:
	var all_mobs = get_all_mobs()
	
	if all_mobs.is_empty():
		return []
	
	var mob_chain: Array[Mob] = []
	var starting_position = global_position
	for i in range(spell.stats.get("BOUNCES").get_value() + 1):
		var best_mob: Mob = null
		for mob in all_mobs:
			if mob_chain.has(mob):
				continue
			var distance = mob.global_position.distance_squared_to(starting_position)
			var max_range = spell.stats.get("BOUNCE_RANGE").get_value()
			if distance > max_range*max_range:
				continue
			if best_mob != null && best_mob.global_position.distance_squared_to(starting_position) < distance:
				continue
			best_mob = mob
		if best_mob == null:
			return mob_chain
		mob_chain.append(best_mob)
		starting_position = best_mob.global_position
	
	return mob_chain
	
	
