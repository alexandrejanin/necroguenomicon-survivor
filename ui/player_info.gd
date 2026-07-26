class_name PlayerInfo
extends Label

@onready var player: Player = $/root/Game/Player

func _process(delta: float) -> void:
	if player == null:
		return
	
	text = "Level %d" % player.level
