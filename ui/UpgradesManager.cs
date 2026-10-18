using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class UpgradesManager : Control
{
    private PackedScene _optionResource;
    private Control _optionsParent;
    private Player _player;
    private Button _rerollButton;

    public override void _Ready()
    {
        _player = GetNode<Player>("/root/Game/Player");
        _optionsParent = GetNode<Control>("Options");
        _rerollButton = GetNode<Button>("RerollButton");
        _rerollButton.Pressed += RollRandomOptions;
        _optionResource = GD.Load<PackedScene>("res://ui/upgrade_option.tscn");
    }

    public override void _Process(double delta)
    {
        if (_player == null)
        {
            GD.Print("player == null");
            return;
        }

        if (_player.Xp >= _player.XpThreshold())
        {
            _player.Xp -= _player.XpThreshold();
            _player.Level += 1;
            ShowUpgradesMenu();
        }
    }

    private static Array<Spell> Spells()
    {
        return
        [
            GD.Load<Spell>("res://spells/dagger/dagger.tres"),
            GD.Load<Spell>("res://spells/sword/sword.tres"),
            GD.Load<Spell>("res://spells/boomerang/boomerang.tres"),
            GD.Load<Spell>("res://spells/orb/orb.tres"),
            GD.Load<Spell>("res://spells/beam/beam.tres"),
            GD.Load<Spell>("res://spells/crystal/crystal.tres"),
            GD.Load<Spell>("res://spells/tentacle_buddy/tentacle_buddy.tres"),
            GD.Load<Spell>("res://spells/tentacle_arm/tentacle_arm.tres"),
            GD.Load<Spell>("res://spells/tentacle_circle/tentacle_circle.tres")
        ];
    }

    private void ShowUpgradesMenu()
    {
        GetTree().Paused = true;
        Visible = true;

        RollRandomOptions();
    }

    private void RollRandomOptions()
    {
        foreach (var node in _optionsParent.GetChildren()) node.QueueFree();

        var spells = Spells();
        for (var i = 0; i < 3; i++)
        {
            var template = spells[GD.RandRange(0, spells.Count - 1)];
            var newOption = _optionResource.Instantiate<UpgradeOption>();
            _optionsParent.AddChild(newOption);
            newOption.CustomMinimumSize = new Vector2(300.0f, 300.0f);

            // Owned spells are deep-duplicated copies (see OnSpellSelected),
            // so match by name: the template instance itself is never owned.
            var owned = FindOwned(template.Name);
            if (owned != null)
            {
                var stats = owned.GetStats();
                if (stats.Count > 0)
                    newOption.SetSpellUpgrade(owned, stats[GD.RandRange(0, stats.Count - 1)]);
                else
                    newOption.SetSpell(template);
            }
            else
            {
                newOption.SetSpell(template);
            }

            newOption.SpellSelected += OnSpellSelected;
            newOption.UpgradeSelected += OnUpgradeSelected;
        }
    }

    private void OnSpellSelected(Spell template)
    {
        // Deep-duplicate so upgrades mutate only this run's copy,
        // never the shared editor .tres (which survives scene reloads
        // in the ResourceLoader cache).
        var copy = (Spell)template.Duplicate(true);
        _player.Spells.Add(copy);
        copy.OnAcquired(_player);
        Close();
    }

    private void OnUpgradeSelected(Stat stat)
    {
        stat.Level += 1;
        Close();
    }

    private Spell FindOwned(string spellName)
    {
        return _player.Spells.FirstOrDefault(owned => owned.Name == spellName);
    }

    private void Close()
    {
        Visible = false;
        GetTree().Paused = false;
    }
}