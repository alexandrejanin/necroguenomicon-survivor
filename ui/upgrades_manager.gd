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

func _process(_delta: float) -> void:
	if player == null:
		print_debug("player == null")
		return
	
	if player.xp >= player.xp_threshold():
		player.xp -= player.xp_threshold()
		player.level += 1
		show_upgrades_menu()

func show_upgrades_menu() -> void:
	get_tree().paused = true
	
	for spell in spells:
		var new_option: UpgradeOption = option_resource.instantiate()
		options_parent.add_child(new_option)
		new_option.custom_minimum_size = Vector2(300, 300)
		
		if player.spells.has(spell):
			new_option.set_spell_upgrade(spell, spell.get_stats().pick_random())
		else:
			new_option.set_spell(spell)
		
		new_option.spell_selected.connect(on_spell_selected)
		new_option.upgrade_selected.connect(on_upgrade_selected)
	
	options_parent.visible = true

func on_spell_selected(spell: Spell) -> void:
	player.spells.append(spell)
	close()

func on_upgrade_selected(stat: Stat) -> void:
	stat.level += 1
	close()

func close() -> void:
	
	options_parent.visible = false
	for node in options_parent.get_children():
		node.queue_free()
	
	get_tree().paused = false
