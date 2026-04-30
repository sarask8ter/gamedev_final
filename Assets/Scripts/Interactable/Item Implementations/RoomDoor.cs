using UnityEngine;

public class RoomDoor : DoorPivot
{
    [SerializeField] ItemName roomItem;
    bool hasCountedVisit;

    protected override void PostOpenOrClose(bool shouldOpen)
    {
        bool firstOpen = !isOpen && shouldOpen;

        if(firstOpen && !hasCountedVisit)
        {
            hasCountedVisit = true;

            TasksEvents.OnItemInteract?.Invoke(roomItem);
        }
    }
}