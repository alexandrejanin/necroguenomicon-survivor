class_name Mob
extends RigidBody2D

@export var SPEED := 10.0

@onready var player: Player = $/root/Game/Player
@onready var sprite := $Sprite
@onready var hitbox := $Hitbox

var xp_res := preload("res://scenes/xp.tscn")

func _ready() -> void:
	hitbox.body_entered.connect(on_hit)

func _integrate_forces(state: PhysicsDirectBodyState2D) -> void:
	if player == null:
		return
	
	state.linear_velocity = SPEED * (position.direction_to(player.position))
	sprite.flip_h = state.linear_velocity.x > 0

func _exit_tree() -> void:
	var xp: Node2D = xp_res.instantiate()
	xp.global_position = global_position
	$/root/Game.add_child.call_deferred(xp)

func on_hit(other: Node):
	if other == player:
		player.take_damage()
