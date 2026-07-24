extends Node

@export
var mobs_per_second = 2

@onready
var mob := preload("res://scenes/mob.tscn")
@onready
var player = %Player

var spawn_timer = 0.0

func _process(delta: float) -> void:
	if player == null:
		printerr("%Player == null")
		return
	spawn_timer += delta
	if spawn_timer >= 1.0 / mobs_per_second:
		spawn_mob()
		spawn_timer = 0.0

func spawn_mob() -> void:
	var new_mob: RigidBody2D  = mob.instantiate()
	add_child(new_mob)
	var angle = randf_range(0, 2 * PI)
	var spawn_position = player.global_position + Vector2(150, 0).rotated(angle)
	new_mob.global_position = spawn_position
