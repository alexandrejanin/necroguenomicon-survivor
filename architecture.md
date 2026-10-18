# Spell system data model (revised; covers all of spells.md)

Ownership rule: Pattern decides how many spawn and where.
Movement decides initial direction + everything after.
A number lives in exactly one place.

- Spell: Resource
  - name, icon
  - FIRE_RATE: Stat, optional (null = PERMANENT, e.g. Orb:
	every frame the runner counts live projectiles of this spell
	and spawns the missing slots until pattern.desired_count()
	is met. Covers spawn-on-acquire, replacement, and count
	upgrades with one rule. No dead field, no magic 0)
  - pattern: FiringPattern
  - projectile: ProjectileData

- FiringPattern: Resource (how many + where; never angles/directions)
  - Returns Array[Vector2] spawn positions (absolute) per cast.
  - Positions are relative to an anchor: the player for casts,
	the trigger position for effects (Dagger explosion rings the
	killed mob, not the player).
  - Only these place away from the anchor (and set the spawn
	rotation, since StationaryMovement keeps it):
	AtRandomEnemy, AtHit.
  - Inputs: player, cast index, mob list, hit context.
  - desired_count() -> int: how many should stay alive for PERMANENT
	spells (base 1; CountPattern returns its count).
  - Subclasses, each with only its own Stats (3 total):
	- CountPattern: count: Stat (Dagger 1, Tentacle circle N,
	Orb slots N, Dagger explosion N, Sword arc 1, Tentacle arm 1)
	- AtRandomEnemyPattern (Tentacle buddy): offset: Vector2
	- AtHitPattern (Piercing dagger): no stats (spawn at hit pos)

- ProjectileData: Resource
  - damage, pierce: Stat (upgradeable; pierce < 0 = infinite, e.g. Orb)
  - lifetime: float, fixed (seconds; range for linears, fuse for
	Crystal; <= 0 = infinite, e.g. Orb)
  - hit_cooldown: float, fixed (min seconds before the same mob is
	hit again; 0 = once per contact; Orb/Beam need ~0.3-0.5)
  - movement: Movement
  - visual: PackedScene (sprite + shape, tweaked in editor)
  - on_hit_effects, on_kill_effects, on_expire_effects: Array[Effect]
  - Expiry rule: pierce exhausted OR lifetime elapsed OR movement
	requests expire() (Boomerang returning home) -> free +
	always fire on_expire_effects.

- Movement: Resource (initial direction + travel)
  - setup(player, spawn_pos, cast_index, spawn_index, spawn_total,
	mobs, hit) sets the initial transform; travel() runs each frame.
  - Subclasses, each with only its own Stats:
	- RotatingMovement (Dagger): angle_step: float, speed: float (both fixed)
	(dir = step * cast_index + TAU * spawn_index / spawn_total (same-cast spread))
	- RadialMovement (Tentacle circle, Dagger explosion):
	speed: float, fixed (0 = stationary facing outward: Tentacle circle)
	- AimNearestMovement (Crystal): speed: float, fixed
	- ThroughMovement (Piercing dagger): speed: float, fixed
	(dir = player->hit point)
	- OrbitMovement (Orb): radius, angular_speed: Stat
	(slot angle = TAU * spawn_index / spawn_total, snaps on setup)
	- StationaryMovement (Fire geyser, Poison cloud, Glacier):
	no stats (world-fixed at spawn transform)
	- FollowMovement (Sword arc, Beam, Tentacle arm): swing_angle: float, fixed
	(glued to player each frame, faces nearest once on spawn; forward offset
	lives in the visual scene)
	- BoomerangMovement: speed: float, fixed; outward_range: Stat;
	catch_radius: float, fixed (aims random mob on setup; flies straight
	out to outward_range, then homes; frees within catch_radius of the
	player)

- Effect: Resource
  - Trigger call: trigger(anchor_pos, player, mobs, hit_mob, killed).
	killed = mob.hp <= 0 after this hit's damage. On a kill both
	lists fire, hit first then kill.
  - Subclasses:
    - SpawnEffect (Piercing dagger, Dagger explosion, Fire geyser):
	pattern + projectile. Anchor = hit/death/expire position.
	Effect projectiles carry their own Stats, so the acquire-time
	deep-duplicate makes them separately upgradeable.
    - ChainEffect (Lightning chain/strike): damage, jumps: Stat,
	visual: PackedScene, duration: float, fixed. Hits nearest jumps mobs
	to the anchor (infinite range, per ruling); one visual arc
	per jump, cosmetic only.
    - AreaEffect (Glacier burst, Lightning strike core, Poison cloud
	flash): damage, radius: Stat, visual: PackedScene,
	duration: float, fixed. Damages all mobs within radius of anchor;
	visual is cosmetic only.
  - Status effects are phase 2 (deferred); Poison cloud / Glacier /
	burns deal their damage portion only until then.

- ProjectileInstance: Node (single generic script, no per-weapon code)
  - Asks pattern for spawns, moves per Movement, counts pierce,
	fires effect lists on hit/kill/expire.

- Ownership / upgrades
  - Player deep-duplicates the Spell (incl. sub-resources) on acquire.
  - Upgrades mutate the copy's Stats only, never the editor .tres.
  - Upgrade screen traverses the copy collecting every Stat sub-resource.
