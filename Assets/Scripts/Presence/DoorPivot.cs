using UnityEngine;
using System;

public class DoorPivot : MonoBehaviour, IInteractable
{
    public bool IsInteractable => true;

    [SerializeField] private float openAngle;
    [SerializeField] private float speed;

    private bool moveInBoxesStarted;
    private bool closeDoorTaskActive;
    private bool isOpen;

    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(0, openAngle, 0);

        ProgressManager.SubscribeToStart(ProgressEvent.MoveInBoxes, () => moveInBoxesStarted = true);
        ProgressManager.SubscribeToStart(ProgressEvent.CloseDoor, () => closeDoorTaskActive = true);
    }

    public void Interact(PlayerInteractor player)
    {
        if (!moveInBoxesStarted) return;

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

    void SetOpen(bool shouldOpen)
    {
        bool wasOpen = isOpen;
        isOpen = shouldOpen;

        StopAllCoroutines();
        StartCoroutine(RotateDoor());
    }

    System.Collections.IEnumerator RotateDoor()
    {
        Quaternion target = isOpen ? openRot : closedRot;

        while (Quaternion.Angle(transform.rotation, target) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                target,
                Time.deltaTime * speed
            );
            yield return null;
        }

        transform.rotation = target;

        // ONLY notify systems, NOT ProgressManager
        if (!isOpen)
        {
            TasksEvents.OnItemInteract?.Invoke(ItemName.Door);
        }
    }

    public void Slam()
    {
        StopAllCoroutines();
        isOpen = false;
        transform.rotation = closedRot;
    }
}