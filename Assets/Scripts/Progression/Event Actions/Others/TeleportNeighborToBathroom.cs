using UnityEngine;

[CreateAssetMenu(fileName = "TeleportNeighborToBathroom", menuName = "Event Actions/Others/Teleport Neighbor to Bathroom")]
public class TeleportNeighborToBathroom : EventAction
{
    public override void OnEventStart()
    {
        MovementHelper.MoveToPoint(GameState.Neighbor, GameState.NeighborBathroomTeleportPoint, false);
    }
}