class_name Spell
extends Resource

@export var name := "Spell"
@export var icon: Texture
@export var cooldown := 5.0
@export var projectile: Resource

var cooldown_timer := 0.0
var projectile_counter := 0

func tick(player: Player, delta: float) -> void:
	cooldown_timer += delta
	if (cooldown_timer >= cooldown):
		var new_projectile: Projectile = projectile.instantiate()
		player.add_sibling(new_projectile)
		if new_projectile.on_shoot(player, projectile_counter):
			cooldown_timer = 0.0
			projectile_counter += 1
		else:
			new_projectile.queue_free()
