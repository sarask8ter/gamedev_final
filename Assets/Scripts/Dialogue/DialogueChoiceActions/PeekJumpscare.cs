using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueChoice - Peek Jumpscare", menuName = "Dialogue/Choice Actions/Peek Jumpscare")]
public class PeekJumpscare : DialogueChoiceAction
{

    public override void Execute()
    {
        MovementHelper.MovePlayer(GameState.PlayerPeekTeleportPoint);
        GameState.FrontDoor.SetOpen(true);
        CoroutineHelper.StartCoroutineHelper(Jumpscare());
    }

    
    IEnumerator Jumpscare()
    {
        var neighbor = GameState.Neighbor;
        var neighborTeleportPoint = GameState.NeighborPeekTeleportPoint;

        // Move neighbor slightly away (away from player/camera direction)
        Vector3 jumpOffset = -neighbor.transform.right.normalized * 6f;
        neighbor.transform.position += jumpOffset;

        yield return new WaitForSeconds(2f);

        // Snap back to original spawn position
        MovementHelper.MoveToPoint(neighbor, neighborTeleportPoint, false);
    }
}
