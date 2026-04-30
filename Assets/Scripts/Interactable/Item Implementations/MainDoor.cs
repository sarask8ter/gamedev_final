using UnityEngine;
using System.Collections;

public class MainDoor : DoorPivot
{
    [SerializeField] private ProgressEvent closeDoorTask;
    private bool closeDoorTaskActive;

    protected override void Start()
    {
        base.Start();
        ProgressManager.SubscribeToStart(closeDoorTask, () => closeDoorTaskActive = true);
    }

    public override void Interact(PlayerInteractor player = null)
    {
        if (isOpen)
        {
            if (!closeDoorTaskActive) return;
            SetOpen(false);
        }
        else
        {
            SetOpen(true);
        }
    }

    public override void SetOpen(bool shouldOpen)
    {
        isOpen = shouldOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoorAndNotify());
    }

    IEnumerator RotateDoorAndNotify()
    {
        yield return RotateDoor();
        // ONLY notify systems, NOT ProgressManager
        if (!isOpen)
        {
            TasksEvents.OnItemInteract?.Invoke(ItemName.HouseDoor);
        }
    }
}