class_name ProjectileData
extends Resource

# Pure data for one projectile type. Tweak in the editor.
@export var damage: Stat
@export var pierce: Stat
@export var lifetime := 4.0
@export var hit_cooldown := 0.0
@export var movement: Movement
@export var visual: PackedScene
@export var on_hit_effects: Array[Effect] = []
@export var on_kill_effects: Array[Effect] = []
@export var on_expire_effects: Array[Effect] = []


func get_stats() -> Array:
	var stats: Array = [damage, pierce]
	if movement != null:
		stats.append_array(movement.get_stats())
	return stats.filter(func(s: Stat) -> bool: return s != null)
