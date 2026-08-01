class_name Player
extends CharacterBody2D

@export var SPEED := 30.0
@export var PICKUP_RANGE := 27.0
@export var MAX_HP := 3
@export var INVINCIBILITY_DURATION := 3.0

var hp := MAX_HP
var level := 0
var xp := 0
var invincibility_timer := INVINCIBILITY_DURATION
var spells: Array[Spell] = []

@onready var sprite := $Sprite

func _process(delta: float) -> void:
	invincibility_timer += delta
	
	sprite.visible = hp > 0 \
				  && (invincibility_timer > INVINCIBILITY_DURATION \
				  || int(invincibility_timer / 0.1) % 2)
	
	if hp <= 0:
		return
	
	for spell in spells:
		spell.tick(self, delta)

func _physics_process(_delta: float) -> void:
	if hp <= 0:
		return
	
	var direction := Input.get_vector("move_left", "move_right", "move_up", "move_down")
	
	velocity = SPEED * direction
	
	move_and_slide()
	
	if velocity.x < 0:
		sprite.flip_h = true
	elif velocity.x > 0:
		sprite.flip_h = false
		

func xp_threshold() -> int:
	return 10 * level

func take_damage(damage: int = 1):
	if hp <= 0 || invincibility_timer < INVINCIBILITY_DURATION:
		return
	
	hp -= damage
	invincibility_timer = 0
