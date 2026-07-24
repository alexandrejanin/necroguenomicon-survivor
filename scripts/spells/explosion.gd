extends Node2D

@export var DURATION := 0.25

var timer := 0.0

func _ready() -> void:
	$Area2D.body_entered.connect(on_enter)
	$AnimatedSprite2D.play()
	
func _process(delta: float) -> void:
	timer += delta
	if timer >= DURATION:
		queue_free()
	
func on_enter(other: Node2D) -> void:
	other.queue_free()
