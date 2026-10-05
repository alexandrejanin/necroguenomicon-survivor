class_name XpPickup
extends Pickup

func on_pickup() -> void:
	player.xp += 1
	$/root/Game/RunManager.total_xp += 1
