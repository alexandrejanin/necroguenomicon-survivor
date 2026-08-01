class_name UpgradeOption
extends Button

var spell: Spell = null
var stat: Stat = null

signal spell_selected(spell: Spell)
signal upgrade_selected(stat: Stat)

func _ready() -> void:
	pressed.connect(on_pressed)
	
func set_spell(new_spell: Spell) -> void:
	spell = new_spell
	$Label.text = spell.name
	$Icon.texture = spell.icon
	
func set_spell_upgrade(new_spell: Spell, new_stat: Stat) -> void:
	spell = new_spell
	stat = new_stat
	$Label.text = "%s: %s\n%f -> %f" % [spell.name, stat.name, stat.get_value(), stat.get_next_value()]
	$Icon.texture = spell.icon

func on_pressed() -> void:
	if stat == null:
		spell_selected.emit(spell)
	else:
		upgrade_selected.emit(stat)
