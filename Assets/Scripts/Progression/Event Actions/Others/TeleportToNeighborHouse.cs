using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportPlayerToNeighborHouse", menuName = "Event Actions/Others/Teleport To Neighbor's")]
public class TeleportToNeighbor : EventAction
{
    [SerializeField] private float fadeDuration;

    public override void OnEventStart()
    {
        CoroutineHelper.StartCoroutineHelper(FadeAndTeleport());
    }

    IEnumerator FadeAndTeleport()
    {
        yield return ScreenFader.FadeOutRoutine(fadeDuration);
        TeleportPlayerAndNeighbor();
        yield return ScreenFader.FadeInRoutine(fadeDuration);
        CompleteEvent();
    }

    void TeleportPlayerAndNeighbor()
    {
        // Teleport neighbor.
        MovementHelper.MoveToPoint(GameState.Neighbor, GameState.NeighborAtNeighborHouseInitialTeleportPoint, false);

        // Teleport player.
        MovementHelper.MovePlayer(GameState.PlayerAtNeighborHouseInitialTeleportPoint);
    }
}