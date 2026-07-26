class_name RunManager
extends Node

var total_xp := 0
var total_kills := 0

func get_text() -> String:
	return "Killed %d enemies\nPicked up %d XP" % [total_kills, total_xp]
