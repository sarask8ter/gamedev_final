using UnityEngine;

[CreateAssetMenu(fileName = "TeleportNeighbor", menuName = "Event Actions/Others/Teleport Neighbor")]
public class TeleportNeighborToBathroom : EventAction
{
    [SerializeField] private bool toBathroom;
    public override void OnEventStart()
    {
        MovementHelper.MoveToPoint(GameState.Neighbor, toBathroom ? GameState.NeighborBathroomTeleportPoint : GameState.NeighborGetOutTeleportPoint, false);
    }
}