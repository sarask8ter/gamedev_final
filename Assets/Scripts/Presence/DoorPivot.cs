using UnityEngine;
using System;

public class DoorPivot : MonoBehaviour, IInteractable
{
    public bool IsInteractable => true;

    [SerializeField] private float openAngle;
    [SerializeField] private float speed;
    [SerializeField] private ItemName doorItem = ItemName.Door;

    private bool moveInBoxesStarted;
    private bool closeDoorTaskActive;
    private bool isOpen;

    private Quaternion closedRot;
    private Quaternion openRot;

    public bool IsOpen => isOpen;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(0, openAngle, 0);

        ProgressManager.SubscribeToStart(ProgressEvent.MoveInBoxes, () => moveInBoxesStarted = true);
        ProgressManager.SubscribeToStart(ProgressEvent.CloseDoor, () => closeDoorTaskActive = true);
    }

    public void Interact(PlayerInteractor player = null)
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

    public void SetOpen(bool shouldOpen)
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

        if (!isOpen)
        {
            Debug.Log("Door closed → firing Door");
            TasksEvents.OnItemInteract?.Invoke(ItemName.Door);
        }

        if (isOpen)
        {
            Debug.Log("Door opened → firing " + doorItem);
            TasksEvents.OnItemInteract?.Invoke(doorItem);
        }
    }

    public void Slam()
    {
        StopAllCoroutines();
        isOpen = false;
        transform.rotation = closedRot;
    }
}