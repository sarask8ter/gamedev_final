using UnityEngine;

public class RoomDoor : MonoBehaviour, IInteractable
{
    [SerializeField] float openAngle = 90f;
    [SerializeField] float speed = 2f;

    [SerializeField] ProgressEvent unlockEvent = ProgressEvent.EnterBedroom;
    [SerializeField] ItemName roomItem;

    bool unlocked;
    bool isOpen;
    bool hasCountedVisit;

    Quaternion closedRot;
    Quaternion openRot;

    public bool IsInteractable => unlocked;

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

        if(firstOpen && !hasCountedVisit)
        {
            hasCountedVisit = true;

            TasksEvents.OnItemInteract?.Invoke(roomItem);

            if (roomItem == ItemName.BedroomDoor)
            {
                Debug.Log("Bedroom entered");

                ProgressManager.CompleteEvent(ProgressEvent.EnterBedroom);
            }
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