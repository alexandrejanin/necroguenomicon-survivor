class_name Player
extends CharacterBody2D

@export var SPEED := 30.0
@export var PICKUP_RANGE := 20.0
@export var MAX_HP := 3
@export var INVINCIBILITY_DURATION := 3.0

@onready var hp := MAX_HP
@onready var xp := 0
@onready var sprite := $Sprite
@onready var spells: Array[Spell] = [preload("res://spells/fireball.tres")]

var invincibility_timer := INVINCIBILITY_DURATION

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
	
	var x_direction := Input.get_axis("ui_left", "ui_right")
	if x_direction:
		velocity.x = x_direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		
	var y_direction := Input.get_axis("ui_up", "ui_down")
	if y_direction:
		velocity.y = y_direction * SPEED
	else:
		velocity.y = move_toward(velocity.y, 0, SPEED)
	
	velocity.limit_length(SPEED)
	
	move_and_slide()
	
	if velocity.x < 0:
		sprite.flip_h = true
	elif velocity.x > 0:
		sprite.flip_h = false

func take_damage(damage: int = 1):
	if hp <= 0 || invincibility_timer < INVINCIBILITY_DURATION:
		return
	
	hp -= damage
	invincibility_timer = 0
