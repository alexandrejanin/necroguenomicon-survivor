class_name XpBar
extends ColorRect

@onready var player: Player = $/root/Game/Player

func _process(delta: float) -> void:
	if player == null:
		print_debug("player == null")
		return
	
	scale.x = float(player.xp) / max(player.xp_threshold(), 1.0)
