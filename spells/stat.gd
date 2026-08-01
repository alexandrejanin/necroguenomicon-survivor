class_name Stat
extends Resource

@export var name: String
@export var base: float
@export var per_level: float

var level := 1

func get_value() -> float:
	return get_value_at(level)

func get_next_value() -> float:
	return get_value_at(level + 1)

func get_value_at(other_level: int) -> float:
	return base + (other_level - 1) * per_level
