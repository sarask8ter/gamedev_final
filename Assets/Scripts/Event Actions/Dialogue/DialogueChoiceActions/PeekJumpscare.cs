using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueChoice - Peek Jumpscare", menuName = "Scriptable Objects/DialogueActions/Peek Jumpscare")]
public class PeekJumpscare : DialogueChoiceAction
{
    private GameObject neighbor;
    private Transform neighborTeleportPoint;
    private GameObject player;
    private Transform playerTeleportPoint;

    public override void Execute()
    {
        neighbor = DialogueContext.Neighbor;
        neighborTeleportPoint = DialogueContext.NeighborPeekTeleportPoint;
    
        player = DialogueContext.Player;
        playerTeleportPoint = DialogueContext.PlayerPeekTeleportPoint;

        TeleportPlayer();
        CoroutineHelper.StartCoroutineHelper(Jumpscare());
    }

    void TeleportPlayer()
    {
        var controller = player.GetComponent<CharacterController>();
        var fps = player.GetComponent<StarterAssets.FirstPersonController>();

        controller.enabled = false;

        MoveAndChangePhysicsMethods.MoveToPoint(player, playerTeleportPoint, false);
        fps.SetLookRotation(
            playerTeleportPoint.eulerAngles.y,
            NormalizePitch(playerTeleportPoint.eulerAngles.x)
        );

        controller.enabled = true;
    }

    float NormalizePitch(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    IEnumerator Jumpscare()
    {
        // Move neighbor slightly away (away from player/camera direction)
        Vector3 jumpOffset = -neighbor.transform.right.normalized * 6f;
        neighbor.transform.position += jumpOffset;

        yield return new WaitForSeconds(1f);

        // Snap back to original spawn position
        MoveAndChangePhysicsMethods.MoveToPoint(neighbor, neighborTeleportPoint, false);
    }
}
