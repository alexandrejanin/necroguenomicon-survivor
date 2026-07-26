class_name DefeatScreen
extends Label

@onready
var player: Player = $/root/Game/Player

func _process(_delta: float) -> void:
	visible = player.hp <= 0
	
	if visible:
		text = "%s\nPress 'R' to try again!" % $/root/Game/RunManager.get_text()
		if Input.is_key_pressed(KEY_R):
			get_tree().reload_current_scene()
