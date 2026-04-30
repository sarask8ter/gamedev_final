using UnityEngine;
using System.Collections;

public class RoomDoor : MonoBehaviour, IInteractable
{
    [SerializeField] float openAngle = 90f;
    [SerializeField] float speed = 2f;

    [SerializeField] ProgressEvent unlockEvent = ProgressEvent.EnterBedroom;
    [SerializeField] ItemName roomItem;
    [SerializeField] private DialogueNode monologue;
    [SerializeField] private PlayerSpeaker player;
    [SerializeField] private SpiritController spirit;

    bool unlocked;
    bool isOpen;
    bool hasCountedVisit;

    Quaternion closedRot;
    Quaternion openRot;

    public bool IsOpen => isOpen;
    public bool IsInteractable => unlocked;
    private PlayerInteractor playerInteractor;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y + openAngle,
            transform.eulerAngles.z
        );

        ProgressManager.SubscribeToStart(
            unlockEvent,
            ()=> unlocked = true
        );

        spirit = FindObjectOfType<SpiritController>();
        playerInteractor = FindObjectOfType<PlayerInteractor>();
    }

    public void Interact(PlayerInteractor player)
    {
        SetOpen(!isOpen);
    }

    void SetOpen(bool shouldOpen)
    {
        bool firstOpen = !isOpen && shouldOpen;

        isOpen = shouldOpen;

        StopAllCoroutines();
        StartCoroutine(RotateDoor());

    if (firstOpen && !hasCountedVisit)
    {
        hasCountedVisit = true;

        if (roomItem != ItemName.FrontDoor)
            TasksEvents.OnItemInteract?.Invoke(roomItem);
        
        if (monologue != null)
            StartCoroutine(RoomSequence());
    }

        if (firstOpen && roomItem == ItemName.BathroomDoor)
        {
            FindObjectOfType<SpiritController>()?.EndBathroomSequence();
            StartCoroutine(DropWithDelay());
        }
    }

    IEnumerator DropWithDelay()
    {
        yield return new WaitForSeconds(0.15f);
        playerInteractor?.ForceDrop();
    }

    IEnumerator RoomSequence()
    {
        yield return new WaitForSeconds(1f);

        PlayerSpeaker ps = player.GetComponent<PlayerSpeaker>();

        if (ps != null && monologue != null)
        {
            ps.StartDialogue(monologue, "You");
        }
    }

    System.Collections.IEnumerator RotateDoor()
    {
        Quaternion target = isOpen ? openRot : closedRot;

        while(Quaternion.Angle(transform.rotation,target) > .1f)
        {
            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    target,
                    Time.deltaTime * speed
                );

            yield return null;
        }

        transform.rotation = target;
    }
}