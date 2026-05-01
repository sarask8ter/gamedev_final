using System.Collections;
using UnityEngine;

public class PeekJumpscare : DialogueChoiceAction
{
    [SerializeField] private GameObject neighbor;
    [SerializeField] private Transform neighborTeleportPoint;
    [SerializeField] private PlayerMover player;
    [SerializeField] private Transform playerPeekTeleportPoint;
    [SerializeField] private DoorPivot frontDoor;

    protected override void Execute()
    {
        player.MovePlayer(playerPeekTeleportPoint);
        frontDoor.SetOpen(true);
        CoroutineHelper.StartCoroutineHelper(Jumpscare());
    }

    IEnumerator Jumpscare()
    {
        neighbor.transform.rotation = neighborTeleportPoint.rotation;

        // Move neighbor slightly away (away from player/camera direction)
        Vector3 jumpOffset = -neighbor.transform.right.normalized * 6f;
        neighbor.transform.position += jumpOffset;

        yield return MoveIn(neighbor, neighborTeleportPoint.position, 1f);
    }

    IEnumerator MoveIn(GameObject neighbor, Vector3 target, float time)
    {
        Vector3 start = neighbor.transform.position;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / time;
            neighbor.transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        neighbor.transform.position = target;
    }
}
