using UnityEngine;
using System.Collections;

public class E1_NeighborEvent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject neighborPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private DoorPivot frontDoor;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerDoorPoint;
    [SerializeField] private PlayerSpeaker playerSpeaker;

    [Header("Dialogue")]
    [SerializeField] private DialogueNode knockDialogueStart;
    [SerializeField] private DialogueNode talkNode;
    [SerializeField] private DialogueNode ignoreNode;

    [Header("Presence")]
    [SerializeField] private SpiritController spiritController;
    [SerializeField] private DialogueNode flickerMonologue;

    private GameObject spawnedNeighbor;
    private bool triggered = false;

    private bool despawnNeighborAfterDialogue = false;

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

        // var jumpscare = FindAnyObjectByType<E1_PeekJumpscare>();
        // if (jumpscare != null)
        // {
        //     jumpscare.SetNeighbor(spawnedNeighbor);
        // }

        var speaker = spawnedNeighbor.GetComponent<Speaker>();
        speaker.StartDialogue(knockDialogueStart, "");
    }


    // public void OnPeekChosen()
    // {
    //     var jumpscare = FindAnyObjectByType<E1_PeekJumpscare>();

    //     if (jumpscare != null)
    //         jumpscare.PlayJumpscare();
    // }

    public void OnTalkChosen()
    {
        despawnNeighborAfterDialogue = true;

        StartCoroutine(TalkSequence());
    }

    IEnumerator TalkSequence()
    {
        yield return StartCoroutine(TeleportPlayer());

        if (frontDoor != null)
            frontDoor.SetOpen(true); // uses SAME logic as E key

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

    public void OnNeighborConversationFinished()
    {
        if (!despawnNeighborAfterDialogue) return;

        despawnNeighborAfterDialogue = false;

        if (frontDoor != null)
            frontDoor.SetOpen(false); // close door

        if (spawnedNeighbor != null)
            spawnedNeighbor.transform.position += Vector3.left * 100f; // neighbor gone
        
        // Trigger haunting sequence
        StartCoroutine(PostNeighborHaunt());
    }

    IEnumerator PostNeighborHaunt()
    {
        yield return new WaitForSeconds(2f);

        if (spiritController != null)
        {
            spiritController.TriggerEvent(SpiritEventType.FlickerLights);
            Debug.Log("Flicker lights");
        }

        yield return new WaitForSeconds(2.5f);

        PlayerSpeaker playerSpeaker = player.GetComponent<PlayerSpeaker>();

        if (playerSpeaker != null && flickerMonologue != null)
        {
            Debug.Log("Monologue starts");

            playerSpeaker.StartDialogue(
                flickerMonologue,
                "You"
            );
        }
    }
}