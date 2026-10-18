using Godot;

[GlobalClass]
public partial class Stat : Resource
{
    [Export] public float Base;

    public int Level = 1;
    [Export] public string Name = "";
    [Export] public float PerLevel;

    public float GetValue()
    {
        return GetValueAt(Level);
    }

    public float GetNextValue()
    {
        return GetValueAt(Level + 1);
    }

    private float GetValueAt(int otherLevel)
    {
        return Base + (otherLevel - 1) * PerLevel;
    }
}