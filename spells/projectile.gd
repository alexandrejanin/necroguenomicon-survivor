# A Projectile is an object that is created by a spell and can collide with mobs.
class_name Projectile
extends RigidBody2D

signal on_hit(mob: Mob, projectile: Projectile)

func _init(data: ProjectileData):
	gravity_scale = 0.0
	collision_layer = Constants.PROJECTILES_LAYER
	collision_mask = Constants.MOB_HURTBOX_LAYER
	
	if data.SHAPE != null:
		var collision_shape = CollisionShape2D.new()
		collision_shape.shape = data.SHAPE
		add_child(collision_shape)
	
	if data.SPRITE != null:
		var sprite = Sprite2D.new()
		sprite.texture = data.SPRITE
		sprite.z_index = Constants.PROJECTILE_Z
		sprite.texture_filter = CanvasItem.TEXTURE_FILTER_NEAREST
		add_child(sprite)
	
	if data.ANIMATION != null:
		var animated_sprite = AnimatedSprite2D.new()
		animated_sprite.animation = data.ANIMATION
		animated_sprite.z_index = Constants.PROJECTILE_Z
		animated_sprite.texture_filter = CanvasItem.TEXTURE_FILTER_NEAREST
		add_child(animated_sprite)

func _get_all_mobs() -> Array[Mob]:
	return $/root/Game/MobSpawner.get_children() as Array[Mob]

func _get_nearest_mob() -> Node2D:
	var nearest_mob: Node2D = null
	for mob in _get_all_mobs():
		if nearest_mob == null \
		   || global_position.distance_squared_to(mob.global_position) \
			< global_position.distance_squared_to(nearest_mob.global_position):
			nearest_mob = mob
	return nearest_mob
