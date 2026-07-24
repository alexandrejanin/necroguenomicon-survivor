class_name Spell
extends Resource

@export var cooldown := 5.0
@export var projectile: Resource

var cooldown_timer := 0.0

func tick(player: Player, delta: float) -> void:
	cooldown_timer += delta
	if (cooldown_timer >= cooldown):
		var new_projectile: Projectile = projectile.instantiate()
		if new_projectile.on_shoot(player):
			player.add_sibling(new_projectile)
			cooldown_timer = 0.0
