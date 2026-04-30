using UnityEngine;

public class MainDoor : DoorPivot
{
    [SerializeField] private ProgressEvent closeDoorTask;
    [SerializeField] private ProgressEvent getOutTask;
    private bool allowClosing;
    private bool allowOpening;

    protected override void PostStart()
    {
        ProgressManager.SubscribeToStart(closeDoorTask, () => {
            allowClosing = true;
            allowOpening = false;
            UpdateInteractableStatus();
        });

        ProgressManager.SubscribeToStart(unlockEvent, () => {
            allowClosing = false;
            allowOpening = true;
            UpdateInteractableStatus();
        });

        ProgressManager.SubscribeToStart(getOutTask, () => {
            allowClosing = false;
            allowOpening = true;
            UpdateInteractableStatus();
        });
        
        // Set up initial.
        allowOpening = false;

        // If we are past unlock event, then set isInteractable.
        if (ProgressManager.HasCompleted(unlockEvent)) {
            Debug.Log("We have completed unlock event for main door");
            var closeDoorTaskPassed = ProgressManager.HasCompleted(closeDoorTask, true);
            var getOutTaskPassed = ProgressManager.HasCompleted(getOutTask, true);
            allowClosing = closeDoorTaskPassed && !getOutTaskPassed;
            allowOpening = getOutTaskPassed ? true : !closeDoorTaskPassed;
        }

        UpdateInteractableStatus(); 
    }
    protected override void PostOpenOrClose(bool shouldOpen)
    {
        UpdateInteractableStatus();
    }

    void UpdateInteractableStatus()
    {
        isInteractable = (isOpen && allowClosing) || (!isOpen && allowOpening);
    }
}