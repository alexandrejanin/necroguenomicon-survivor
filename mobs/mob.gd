class_name Mob
extends RigidBody2D

@export var SPEED := 10.0
@export var MAX_HP := 100

var hp := MAX_HP
var status_effects: Array[StatusEffect]

@onready var player: Player = $/root/Game/Player
@onready var sprite: Sprite2D = $Sprite
@onready var hitbox := $Hitbox
@onready var hurtbox := $Hurtbox

var xp_res := preload("res://xp/xp.tscn")

func _ready() -> void:
	hitbox.body_entered.connect(on_hit)
	hurtbox.body_entered.connect(on_hurt)

func _process(delta: float) -> void:
	status_effects.filter(
		func (effect: StatusEffect):
			return effect.tick(delta, self)
	)
	
	sprite.modulate = Color(2, 0.5, 0.5).lerp(Color(1, 1, 1), sqrt(float(hp) / MAX_HP))

func _integrate_forces(state: PhysicsDirectBodyState2D) -> void:
	if player == null:
		return
	
	state.linear_velocity = SPEED * (position.direction_to(player.position))
	sprite.flip_h = state.linear_velocity.x > 0

func _exit_tree() -> void:
	var xp: Node2D = xp_res.instantiate()
	xp.global_position = global_position
	$/root/Game.add_child.call_deferred(xp)
	$/root/Game/RunManager.total_kills += 1

func on_hit(other: Node):
	if other == player:
		player.take_damage()

func on_hurt(other: Node):
	if other is Projectile:
		var projectile = other as Projectile
		projectile.on_hit.emit(self, projectile)

func take_damage(damage: int):
	hp -= damage
	if hp <= 0:
		queue_free()
