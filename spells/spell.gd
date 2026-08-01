class_name Spell
extends Resource

@export var name := "Spell"
@export var icon: Texture
@export var projectile: Resource
@export var FIRE_RATE: Stat
@export var stats: Dictionary[String, Stat]

var cooldown_timer := 0.0
var projectile_counter := 0

func tick(player: Player, delta: float) -> void:
	cooldown_timer += delta
	if (cooldown_timer >= 1.0 / FIRE_RATE.get_value()):
		var new_projectile: Projectile = projectile.instantiate()
		player.add_sibling(new_projectile)
		if new_projectile.on_shoot(player, self, projectile_counter):
			cooldown_timer = 0.0
			projectile_counter += 1
		else:
			new_projectile.queue_free()

func get_stats() -> Array[Stat]:
	return [FIRE_RATE] + stats.values()
