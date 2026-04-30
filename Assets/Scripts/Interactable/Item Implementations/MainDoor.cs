using UnityEngine;
using System.Collections;

public class MainDoor : DoorPivot
{
    [SerializeField] private ProgressEvent closeDoorTask;
    [SerializeField] private ProgressEvent getOutTask;
    private bool allowClosing;
    private bool allowOpening;

    protected override void Start()
    {
        base.Start();
        ProgressManager.SubscribeToStart(closeDoorTask, () => {
            allowClosing = true;
            allowOpening = false;
        });

        ProgressManager.SubscribeToStart(unlockEvent, () => allowOpening = true);
        ProgressManager.SubscribeToStart(getOutTask, () => allowOpening = true);

        // If we are past unlock event, then set isInteractable.
        if (ProgressManager.HasCompleted(unlockEvent)) {
            Unlock();
            if (!ProgressManager.HasCompleted(closeDoorTask)) allowOpening = true;
        }
    }
    public override void Interact(PlayerInteractor player = null)
    {   
        if (isOpen)
        {
            // Don't allow closing until close door task is activated.
            if (!allowClosing) return;
            SetOpen(false);
        }
        else
        {
            if (!allowOpening) return;
            SetOpen(true);
        }
    }

    protected override IEnumerator RotateDoor()
    {
        TasksEvents.OnItemInteract?.Invoke(ItemName.HouseDoor);
        yield return base.RotateDoor();
    }
}