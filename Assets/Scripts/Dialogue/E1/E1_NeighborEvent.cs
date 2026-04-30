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
    [SerializeField] private DialogueNode pizzaMonologue;

    [Header("Get Out Sequence")]
    [SerializeField] private Transform outsideSpawnPoint;
    [SerializeField] private Transform playerOutsidePoint;
    [SerializeField] private DialogueNode rescueDialogue;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private float exitDistance = 1.5f;

    private GameObject spawnedNeighbor;
    private bool triggered = false;
    private bool despawnNeighborAfterDialogue = false;
    private bool didTalk;
    private bool hasExited;
    private bool exitProcessing;

    void OnEnable()
    {
        ProgressManager.SubscribeToStart(ProgressEvent.DoorKnock, TriggerNeighborEvent);
        ProgressManager.SubscribeToStart(ProgressEvent.EnterBathroom, SpawnNeighborOutsideEarly);
    }

    void TriggerNeighborEvent()
    {
        if (triggered) return;
        triggered = true;

        if (spawnedNeighbor == null)
        {
            spawnedNeighbor = Instantiate(
                neighborPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );
        }
        else
        {
            spawnedNeighbor.transform.SetPositionAndRotation(
                spawnPoint.position,
                spawnPoint.rotation
            );
        }

        var speaker = spawnedNeighbor.GetComponent<Speaker>();
        speaker.StartDialogue(knockDialogueStart, "");
    }

    void Update()
    {
        if (hasExited || exitProcessing) return;
        if (frontDoor == null || !frontDoor.IsOpen) return;

        if (ProgressManager.Instance.CurrentEvent != ProgressEvent.ExploreHouse &&
            ProgressManager.Instance.CurrentEvent != ProgressEvent.GetOut)
            return;

        float dist = Vector3.Distance(player.position, exitPoint.position);

        if (dist < exitDistance)
        {
            exitProcessing = true;
            hasExited = true;

            StartCoroutine(GetOutSequence());
        }
    }

    public void OnTalkChosen()
    {
        despawnNeighborAfterDialogue = true;
        didTalk = true;
        StartCoroutine(TalkPath());
    }

    IEnumerator TalkPath()
    {
        yield return StartCoroutine(TeleportPlayer());

        if (frontDoor != null)
            frontDoor.SetOpen(true);

        var speaker = spawnedNeighbor.GetComponent<Speaker>();
        speaker.StartDialogue(talkNode, "");
    }

    public void OnIgnoreChosen()
    {
        despawnNeighborAfterDialogue = true;
        didTalk = false;
        StartCoroutine(IgnorePath());
    }

    IEnumerator IgnorePath()
{
        var speaker = spawnedNeighbor.GetComponent<Speaker>();
        speaker.StartDialogue(ignoreNode, "");
        yield break;
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
            frontDoor.SetOpen(false);

        if (spawnedNeighbor != null)
            spawnedNeighbor.transform.position += Vector3.left * 100f;

        StartCoroutine(PostNeighborCleanup());
    }

    IEnumerator PostNeighborCleanup()
    {
        yield return new WaitForSeconds(1f);

        if (didTalk)
        {
            yield return StartCoroutine(HauntSequence());
        }

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(PizzaSequence());
    }

    IEnumerator HauntSequence()
    {
        if (spiritController != null)
        {
            spiritController.TriggerEvent(SpiritEventType.FlickerLights);
            Debug.Log("Flicker lights");
        }

        yield return new WaitForSeconds(0.5f);

        PlayerSpeaker ps = player.GetComponent<PlayerSpeaker>();

        if (ps != null && flickerMonologue != null)
        {
            ps.StartDialogue(flickerMonologue, "You");
            
            float timeout = 10f;
            yield return new WaitUntil(() => 
                PlayerStateManager.State == PlayerState.Normal || 
                (timeout -= Time.deltaTime) <= 0f
            );
        }
    }

    IEnumerator PizzaSequence()
    {
        PlayerSpeaker ps = player.GetComponent<PlayerSpeaker>();

        if (ps != null && pizzaMonologue != null)
        {
            ps.StartDialogue(pizzaMonologue, "You");
            
            float timeout = 10f;
            yield return new WaitUntil(() => 
                PlayerStateManager.State == PlayerState.Normal || 
                (timeout -= Time.deltaTime) <= 0f
            );

            Debug.Log("Monologue finished");
        }
        
        ProgressManager.CompleteEvent(ProgressEvent.DoorKnock);
    }

    IEnumerator GetOutSequence()
    {
        exitProcessing = true;
        hasExited = true;

        if (spawnedNeighbor == null)
        {
            spawnedNeighbor = Instantiate(
                neighborPrefab,
                outsideSpawnPoint.position,
                outsideSpawnPoint.rotation
            );
        }
        else
        {
            spawnedNeighbor.transform.SetPositionAndRotation(
                outsideSpawnPoint.position,
                outsideSpawnPoint.rotation
            );
        }

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(TeleportPlayerOutside());

        var speaker = spawnedNeighbor.GetComponent<Speaker>();
        speaker.StartDialogue(rescueDialogue, "");

        // 🔥 WAIT FOR DIALOGUE TO FINISH BEFORE COMPLETING EVENT
        yield return new WaitUntil(() => PlayerStateManager.State == PlayerState.Normal);

        Debug.Log("GetOut sequence complete");

        ProgressManager.CompleteEvent(ProgressEvent.GetOut);
        TasksEvents.OnItemInteract?.Invoke(ItemName.FrontDoor);

        exitProcessing = false;
    }

    IEnumerator TeleportPlayerOutside()
    {
        var controller = player.GetComponent<CharacterController>();
        controller.enabled = false;

        player.SetPositionAndRotation(
            playerOutsidePoint.position,
            playerOutsidePoint.rotation
        );

        yield return null;

        controller.enabled = true;
    }

    public void TriggerOutsideSequence()
    {
        Debug.Log("TRIGGER OUTSIDE SEQUENCE CALLED");

        if (hasExited) return;
        hasExited = true;

        StartCoroutine(GetOutSequence());
    }

    void SpawnNeighborOutsideEarly()
    {
        if (spawnedNeighbor == null)
        {
            spawnedNeighbor = Instantiate(
                neighborPrefab,
                outsideSpawnPoint.position,
                outsideSpawnPoint.rotation
            );
        }
        else
        {
            spawnedNeighbor.transform.SetPositionAndRotation(
                outsideSpawnPoint.position,
                outsideSpawnPoint.rotation
            );
        }
    }
}