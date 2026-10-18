using Godot;

[GlobalClass]
public partial class HeartPickup : Pickup
{
    protected override void OnPickup()
    {
        Player.Hp += 1;
        GetNode<RunManager>("/root/Game/RunManager").TotalXp += 1;
    }
}