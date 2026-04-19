using UnityEngine;

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

    private GameObject spawnedNeighbor;
    private bool triggered = false;

    [SerializeField] private DialogueNode talkNode;
    [SerializeField] private DialogueNode ignoreNode;

    void OnEnable() // 🔥 better than Start
    {
        ProgressManager.SubscribeToStart(ProgressEvent.DoorKnock, TriggerNeighborEvent);
    }

    void TriggerNeighborEvent()
    {
        if (triggered) return;
        triggered = true;

        Debug.Log("KNOCK KNOCK KNOCK");

        spawnedNeighbor = Instantiate(neighborPrefab, spawnPoint.position, spawnPoint.rotation);

        var jumpscare = FindObjectOfType<E1_PeekJumpscare>();
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
        MovePlayerToDoor();

        var jumpscare = FindObjectOfType<E1_PeekJumpscare>();
        if (jumpscare != null)
        {
            jumpscare.PlayJumpscare();
        }
    }

    public void OnTalkChosen()
    { 
        MovePlayerToDoor();

        var speaker = spawnedNeighbor.GetComponent<Speaker>();
        speaker.StartDialogue(talkNode, "");
    }

    public void OnIgnoreChosen()
    {
        var speaker = spawnedNeighbor.GetComponent<Speaker>();
        speaker.StartDialogue(ignoreNode, "");
    }

    void MovePlayerToDoor()
    {
        player.position = playerDoorPoint.position;
        player.rotation = playerDoorPoint.rotation;
    }
}