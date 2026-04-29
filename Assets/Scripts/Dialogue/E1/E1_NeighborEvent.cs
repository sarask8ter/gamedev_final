using UnityEngine;
using System.Collections;

public class E1_NeighborEvent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject neighborPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private MainDoor frontDoor;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerDoorPoint;

    [Header("Dialogue")]
    [SerializeField] private DialogueNode knockDialogueStart;
    [SerializeField] private DialogueNode talkNode;
    [SerializeField] private DialogueNode ignoreNode;

    [Header("Presence")]
    [SerializeField] private SpiritController spiritController;
    [SerializeField] private DialogueNode flickerMonologue;
    [SerializeField] private DialogueNode pizzaMonologue;

    private GameObject spawnedNeighbor;
    private bool triggered = false;
    private bool despawnNeighborAfterDialogue = false;
    private bool didTalk;

    void OnEnable()
    {
        ProgressManager.SubscribeToStart(ProgressEvent.DoorKnock, TriggerNeighborEvent);
    }

    void TriggerNeighborEvent()
    {
        if (triggered) return;
        triggered = true;

        spawnedNeighbor = Instantiate(neighborPrefab, spawnPoint.position, spawnPoint.rotation);

        DialogueManager.StartDialogue(knockDialogueStart);
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

        DialogueManager.StartDialogue(talkNode);
    }

    public void OnIgnoreChosen()
    {
        despawnNeighborAfterDialogue = true;
        didTalk = false;
        StartCoroutine(IgnorePath());
    }

    IEnumerator IgnorePath()
{
        DialogueManager.StartDialogue(ignoreNode);
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

        if (flickerMonologue != null)
        {
            DialogueManager.StartDialogue(flickerMonologue);
            yield return new WaitUntil(() => PlayerStateManager.State == PlayerState.Normal);
        }
    }

    IEnumerator PizzaSequence()
    {
        if (pizzaMonologue != null)
        {
            DialogueManager.StartDialogue(pizzaMonologue);
            yield return new WaitUntil(() => PlayerStateManager.State == PlayerState.Normal);

            Debug.Log("Monologue finished");
        }
        
        ProgressManager.CompleteEvent(ProgressEvent.DoorKnock);
    }
}