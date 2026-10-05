class_name Projectile
extends RigidBody2D

# Generic runner: built from a Spell's ProjectileData, behaves per Movement,
# fires Effect lists on hit/kill/expire. No per-weapon code lives here.
var player: Player = null
var spell: Spell = null
var data: ProjectileData = null
var age := 0.0
var pierce_left := 0
var pierce_infinite := false
var slot := 0
var last_hit: Dictionary = {}


func setup(new_player: Player, new_spell: Spell, spawn_pos: Vector2, cast_index: int, spawn_index: int, spawn_total: int, mobs: Array, hit: Vector2) -> void:
	player = new_player
	spell = new_spell
	data = spell.projectile
	slot = spawn_index
	var max_pierce := 0
	if data.pierce != null:
		max_pierce = int(data.pierce.get_value())
	pierce_infinite = max_pierce < 0
	pierce_left = max_pierce
	data.movement.setup(self, player, spawn_pos, cast_index, spawn_index, spawn_total, mobs, hit)


func _process(delta: float) -> void:
	if data == null:
		return
	age += delta
	# lifetime <= 0 = infinite (Orb).
	if data.lifetime > 0.0 and age >= data.lifetime:
		_expire(global_position)
		return
	data.movement.travel(self, delta)


func on_hit(mob: Mob) -> void:
	if data == null or mob == null or not is_instance_valid(mob):
		return
	var cooldown := data.hit_cooldown
	var id := mob.get_instance_id()
	var now := Time.get_ticks_msec()
	if last_hit.has(id) and float(now - last_hit[id]) / 1000.0 < cooldown:
		return
	last_hit[id] = now
	mob.take_damage(_damage())
	var anchor := mob.global_position
	for effect in data.on_hit_effects:
		effect.trigger(anchor, player, _mobs(), mob, false)
	if mob.hp <= 0:
		for effect in data.on_kill_effects:
			effect.trigger(anchor, player, _mobs(), mob, true)
	if pierce_infinite:
		return
	pierce_left -= 1
	if pierce_left < 0:
		_expire(anchor)


func _damage() -> int:
	if data.damage == null:
		return 1
	return int(data.damage.get_value())


func _expire(anchor: Vector2) -> void:
	for effect in data.on_expire_effects:
		effect.trigger(anchor, player, _mobs(), null, false)
	queue_free()


func _mobs() -> Array:
	if player == null:
		return []
	return player.get_node("%MobSpawner").get_children()
