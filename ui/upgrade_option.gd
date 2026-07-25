class_name UpgradeOption
extends Button

var spell: Spell = null

signal selected(spell: Spell)

func _ready() -> void:
	pressed.connect(on_pressed)
	
func set_spell(new_spell: Spell) -> void:
	spell = new_spell
	$Label.text = spell.name
	$Icon.texture = spell.icon

func on_pressed() -> void:
	selected.emit(spell)
