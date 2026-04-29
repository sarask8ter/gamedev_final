using UnityEngine;

public class RoomDoor : DoorPivot
{
    [SerializeField] ItemName roomItem;
    bool hasCountedVisit;

    public override void SetOpen(bool shouldOpen)
    {
        bool firstOpen = !isOpen && shouldOpen;

        isOpen = shouldOpen;

        StopAllCoroutines();
        StartCoroutine(RotateDoor());

        if(firstOpen && !hasCountedVisit)
        {
            hasCountedVisit = true;

            TasksEvents.OnItemInteract?.Invoke(roomItem);

            ProgressManager.CompleteEvent(unlockEvent);
        }
    }
}