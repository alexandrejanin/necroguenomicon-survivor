extends Sprite2D

@export var ANIMATION_DURATION := 0.5

@onready var player: Player = $/root/Game/Player
var animation_curve: Curve = preload("res://xp_animation_curve.tres")

var animation_timer := 0.0
var in_range := false

var starting_position: Vector2

func _process(delta: float) -> void:
	if player == null:
		printerr("player == null")
		return
	
	if !in_range && global_position.distance_squared_to(player.global_position) < player.PICKUP_RANGE * player.PICKUP_RANGE:
		in_range = true
		starting_position = global_position

	if in_range:
		animation_timer += delta
		var t = animation_timer / ANIMATION_DURATION
		global_position = starting_position.lerp(player.global_position, animation_curve.sample(t))
		var whiteness = max(10 * t, 1)
		modulate = Color(whiteness, whiteness, whiteness)
		
		if t >= 1:
			player.xp += 1
			queue_free()
