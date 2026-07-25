class_name UpgradesManager
extends Control

var spells: Array[Spell] = [
	preload("res://spells/fireball/fireball.tres"),
	preload("res://spells/dagger/dagger.tres"),
	preload("res://spells/lightning/lightning.tres")
]

var option_resource := preload("res://ui/upgrade_option.tscn")

@onready var player: Player = $/root/Game/Player
@onready var options_parent: Control = $Options
@onready var xp_bar: Control = $XpBar

func _process(_delta: float) -> void:
	if player == null:
		print_debug("player == null")
		return
	
	xp_bar.scale.x = float(player.xp) / max(xp_threshold(player.level), 1.0)
	
	if player.xp >= xp_threshold(player.level):
		player.xp -= xp_threshold(player.level)
		player.level += 1
		show_upgrades_menu()

func xp_threshold(level: int) -> int:
	return 10 * level
	
func show_upgrades_menu() -> void:
	get_tree().paused = true
	
	for spell in spells:
		var new_option: UpgradeOption = option_resource.instantiate()
		options_parent.add_child(new_option)
		new_option.custom_minimum_size.x = 100
		new_option.set_spell(spell)
		new_option.selected.connect(on_option_selected)
	
	options_parent.visible = true

func on_option_selected(spell: Spell) -> void:
	player.spells.append(spell)
	
	options_parent.visible = false
	for node in options_parent.get_children():
		node.queue_free()
	
	get_tree().paused = false
