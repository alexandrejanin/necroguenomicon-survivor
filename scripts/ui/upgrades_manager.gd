class_name UpgradesManager
extends Control

@onready var player: Player = $/root/Game/Player
@onready var options_parent: Control = $Options
@onready var xp_bar: Control = $XpBar

@export var spells: Array[Spell] = [
	preload("res://spells/fireball.tres"),
	preload("res://spells/dagger.tres")
]

func _ready() -> void:
	for option: UpgradeOption in options_parent.get_children():
		option.selected.connect(on_option_selected)

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
	
	for option: UpgradeOption in options_parent.get_children():
		option.set_spell(spells.pick_random())
	
	options_parent.visible = true

func on_option_selected(spell: Spell) -> void:
	player.spells.append(spell)
	options_parent.visible = false
	get_tree().paused = false
