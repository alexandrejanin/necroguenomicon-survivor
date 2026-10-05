class_name Spell
extends Resource

@export var name := "Spell"
@export var icon: Texture
@export var FIRE_RATE: Stat
@export var pattern: FiringPattern
@export var projectile: ProjectileData

var cooldown_timer := 0.0
var projectile_counter := 0


func tick(player: Player, delta: float) -> void:
	# No FIRE_RATE = PERMANENT: keep the target alive instead of casting.
	if FIRE_RATE == null:
		_top_up(player)
		return
	cooldown_timer += delta
	if cooldown_timer < 1.0 / FIRE_RATE.get_value():
		return
	cast(player, player.global_position, Vector2.ZERO)
	cooldown_timer = 0.0
	projectile_counter += 1


func on_acquired(_player: Player) -> void:
	# Nothing to do: the next tick tops up PERMANENT spells,
	# TIMED spells start casting on cooldown.
	pass


# PERMANENT upkeep: spawn whichever desired slots have no live projectile.
# Covers first spawn, replacement, and count upgrades with one rule.
func _top_up(player: Player) -> void:
	if pattern == null or projectile == null or projectile.visual == null:
		return
	if player.get_parent() == null:
		return
	var desired := pattern.desired_count()
	if desired <= 0:
		return
	var live := {}
	for sibling in player.get_parent().get_children():
		if sibling is Projectile and sibling.spell == self:
			live[sibling.slot] = true
	if live.size() >= desired:
		return
	var mobs: Array = player.get_node("%MobSpawner").get_children()
	for i in desired:
		if not live.has(i):
			_spawn(player, player.global_position, Vector2.ZERO, i, desired, mobs)


func cast(player: Player, anchor: Vector2, hit: Vector2) -> void:
	if pattern == null or projectile == null or projectile.visual == null:
		return
	var mobs: Array = player.get_node("%MobSpawner").get_children()
	var spawns := pattern.get_spawns(anchor, player, projectile_counter, mobs, hit)
	var total := spawns.size()
	for i in total:
		_spawn(player, spawns[i], hit, i, total, mobs)


func _spawn(player: Player, spawn_pos: Vector2, hit: Vector2, spawn_index: int, spawn_total: int, mobs: Array) -> void:
		var instance := projectile.visual.instantiate() as Projectile
		if instance == null:
			return
		player.add_sibling(instance)
		instance.setup(player, self, spawn_pos, projectile_counter, spawn_index, spawn_total, mobs, hit)


func get_stats() -> Array:
	var stats: Array = []
	if FIRE_RATE != null && FIRE_RATE.per_level > 0:
		stats.append(FIRE_RATE)
	if pattern != null:
		stats.append_array(pattern.get_stats().filter(func(stat): return stat.per_level > 0))
	if projectile != null:
		stats.append_array(projectile.get_stats().filter(func(stat): return stat.per_level > 0))
	return stats
