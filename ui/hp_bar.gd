class_name HpBar
extends TextureRect

@export var WIDTH_PER_HP := 16

@onready var player: Player = $/root/Game/Player

func _process(_delta: float) -> void:
	if player == null:
		printerr("player == null")
		return
	
	size.x = WIDTH_PER_HP * player.hp
	visible = player.hp > 0
