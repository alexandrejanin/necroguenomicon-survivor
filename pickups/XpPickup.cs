using Godot;

[GlobalClass]
public partial class XpPickup : Pickup
{
    protected override void OnPickup()
    {
        Player.Xp += 1;
        GetNode<RunManager>("/root/Game/RunManager").TotalXp += 1;
    }
}