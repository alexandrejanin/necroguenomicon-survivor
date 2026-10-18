using Godot;

public partial class MobSpawner : Node2D
{
    [Export] public float MobsPerSecond = 2.0f;
    private PackedScene _mob;
    private Node2D _player;

    private double _spawnTimer;

    public override void _Ready()
    {
        _mob = GD.Load<PackedScene>("res://mobs/mob.tscn");
        _player = GetNode<Node2D>("%Player");
    }

    public override void _Process(double delta)
    {
        if (_player == null)
        {
            GD.PrintErr("%Player == null");
            return;
        }

        _spawnTimer += delta;
        if (_spawnTimer >= 1.0 / MobsPerSecond)
        {
            SpawnMob();
            _spawnTimer = 0.0;
        }
    }

    private void SpawnMob()
    {
        var newMob = _mob.Instantiate<RigidBody2D>();
        AddChild(newMob);
        var angle = (float)GD.RandRange(0.0f, 2.0f * Mathf.Pi);
        var spawnPosition = _player.GlobalPosition + new Vector2(150.0f, 0.0f).Rotated(angle);
        newMob.GlobalPosition = spawnPosition;
    }
}