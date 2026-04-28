using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportPlayerToNeighborHouse", menuName = "Event Actions/Others/Teleport To Neighbor's")]
public class TeleportToNeighbor : EventAction
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private bool teleportForTea;

    public override void OnEventStart()
    {
        CoroutineHelper.StartCoroutineHelper(FadeAndTeleport());
    }

    IEnumerator FadeAndTeleport()
    {
        PlayerStateManager.State = PlayerState.NoInput;

        yield return ScreenFader.FadeOut(fadeDuration);

        TeleportPlayerAndNeighbor();
        if (teleportForTea) foreach (var obj in GameState.ObjectsToActivateAtTeaReady) obj.SetActive(true);

        yield return ScreenFader.FadeIn(fadeDuration);

        PlayerStateManager.State = PlayerState.Normal;
        CompleteEvent();
    }

    void TeleportPlayerAndNeighbor()
    {
        // Teleport neighbor.
        MovementHelper.MoveToPoint(GameState.Neighbor, teleportForTea ? GameState.NeighborTeaTeleportPoint : GameState.NeighborAtNeighborHouseInitialTeleportPoint, false);

        // Teleport player.
        MovementHelper.MovePlayer(teleportForTea ? GameState.PlayerTeaTeleportPoint : GameState.PlayerAtNeighborHouseInitialTeleportPoint);
    }
}