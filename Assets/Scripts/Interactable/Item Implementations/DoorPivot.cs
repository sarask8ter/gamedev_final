using UnityEngine;

public class DoorPivot : MonoBehaviour, IInteractable
{
    protected bool isInteractable;
    public bool IsInteractable => isInteractable;
    [SerializeField] protected ProgressEvent unlockEvent;
    [SerializeField] private float openAngle;
    [SerializeField] private float speed;
    [SerializeField] private bool isBloodiedDoor;
    [SerializeField] private bool singleTimeInteract;

    protected bool isOpen;
    private Quaternion closedRot;
    private Quaternion openRot;

    protected virtual void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(0, openAngle, 0);

        ProgressManager.SubscribeToStart(unlockEvent, Unlock);
    }

    public void Unlock()
    {
        isInteractable = true;
    }

    public virtual void Interact(PlayerInteractor player)
    {
        SetOpen(!isOpen);
        if (isBloodiedDoor) TasksEvents.OnItemInteract?.Invoke(ItemName.BloodiedDoor);
        if (singleTimeInteract) isInteractable = false;
    }

    public void Slam()
    {
        isOpen = false;
        StopAllCoroutines();
        transform.rotation = closedRot;
    }

    public void Open()
    {
        SetOpen(true);
    }

    public void Close()
    {
        SetOpen(false);
    }

    public virtual void SetOpen(bool shouldOpen)
    {
        isOpen = shouldOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor());
        Debug.Log("Door rotating. isOpen = " + isOpen);
    }

    protected System.Collections.IEnumerator RotateDoor()
    {
        Debug.Log("Rotating door");
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
    }
}
