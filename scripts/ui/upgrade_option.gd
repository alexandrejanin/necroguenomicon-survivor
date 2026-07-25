class_name UpgradeOption
extends Button

var spell: Spell = null

@onready var label := $Label

signal selected(spell: Spell)

func _ready() -> void:
	pressed.connect(on_pressed)
	
func set_spell(new_spell: Spell) -> void:
	spell = new_spell
	label.text = spell.name

func on_pressed() -> void:
	selected.emit(spell)
