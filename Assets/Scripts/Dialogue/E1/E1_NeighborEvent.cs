using UnityEngine;
using System.Collections;

public class E1_NeighborEvent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject neighborPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerDoorPoint;

    [Header("Dialogue")]
    [SerializeField] private DialogueNode knockDialogueStart;
    [SerializeField] private DialogueNode talkNode;
    [SerializeField] private DialogueNode ignoreNode;

    private GameObject spawnedNeighbor;
    private bool triggered = false;

    void OnEnable()
    {
        ProgressManager.SubscribeToStart(ProgressEvent.DoorKnock, TriggerNeighborEvent);
    }

    void TriggerNeighborEvent()
    {
        if (triggered) return;
        triggered = true;

        Debug.Log("KNOCK KNOCK KNOCK");

        spawnedNeighbor = Instantiate(neighborPrefab, spawnPoint.position, spawnPoint.rotation);

        var jumpscare = FindAnyObjectByType<E1_PeekJumpscare>();
        if (jumpscare != null)
        {
            jumpscare.SetNeighbor(spawnedNeighbor);
        }

        var speaker = spawnedNeighbor.GetComponent<Speaker>();
        speaker.StartDialogue(knockDialogueStart, "");

        ProgressManager.CompleteEvent(ProgressEvent.DoorKnock);
    }


    public void OnPeekChosen()
    {
        StartCoroutine(TeleportPlayer());

        var jumpscare = FindAnyObjectByType<E1_PeekJumpscare>();
        if (jumpscare != null)
        {
            jumpscare.PlayJumpscare();
        }
    }

    public void OnTalkChosen()
    { 
        StartCoroutine(TeleportPlayer());

        var speaker = spawnedNeighbor.GetComponent<Speaker>();
        speaker.StartDialogue(talkNode, "");
    }

    public void OnIgnoreChosen()
    {
        var speaker = spawnedNeighbor.GetComponent<Speaker>();
        speaker.StartDialogue(ignoreNode, "");
    }

    IEnumerator TeleportPlayer()
    {
        var controller = player.GetComponent<CharacterController>();
        var fps = player.GetComponent<StarterAssets.FirstPersonController>();

        // Disable controller so it doesn't override position
        controller.enabled = false;

        // Reset velocity so no snap-back
        if (fps != null)
        {
            var verticalVelField = typeof(StarterAssets.FirstPersonController)
                .GetField("_verticalVelocity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (verticalVelField != null)
            {
                verticalVelField.SetValue(fps, 0f);
            }
        }

        // Teleport
        player.SetPositionAndRotation(playerDoorPoint.position, playerDoorPoint.rotation);

        yield return null; // wait 1 frame

        controller.enabled = true;
    }
}