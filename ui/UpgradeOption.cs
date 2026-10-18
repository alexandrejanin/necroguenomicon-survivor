using Godot;

[GlobalClass]
public partial class UpgradeOption : Button
{
    [Signal]
    public delegate void SpellSelectedEventHandler(Spell spell);

    [Signal]
    public delegate void UpgradeSelectedEventHandler(Stat stat);

    private Spell _spell;
    private Stat _stat;

    public override void _Ready()
    {
        Pressed += OnPressed;
    }

    public void SetSpell(Spell newSpell)
    {
        _spell = newSpell;
        GetNode<Label>("Label").Text = _spell.Name;
        GetNode<TextureRect>("Icon").Texture = _spell.Icon;
    }

    public void SetSpellUpgrade(Spell newSpell, Stat newStat)
    {
        _spell = newSpell;
        _stat = newStat;
        GetNode<Label>("Label").Text = $"{_spell.Name}: {_stat.Name}\n{_stat.GetValue()} -> {_stat.GetNextValue()}";
        GetNode<TextureRect>("Icon").Texture = _spell.Icon;
    }

    private void OnPressed()
    {
        if (_stat == null)
            EmitSignal(SignalName.SpellSelected, _spell);
        else
            EmitSignal(SignalName.UpgradeSelected, _stat);
    }
}