class_name HeartPickup
extends Pickup

func on_pickup() -> void:
	player.hp += 1
	$/root/Game/RunManager.total_xp += 1
