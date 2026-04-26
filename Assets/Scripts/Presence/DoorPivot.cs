using UnityEngine;

public class DoorPivot : MonoBehaviour, IInteractable
{
    public bool IsInteractable => true;

    [SerializeField] private float openAngle;
    [SerializeField] private float speed;

    private bool closeDoorTaskActive;
    private bool isOpen;

    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(0, openAngle, 0);

        ProgressManager.SubscribeToStart(ProgressEvent.CloseDoor, EnableCloseDoorTask);
    }

    void EnableCloseDoorTask()
    {
        closeDoorTaskActive = true;
    }

    public void Interact(PlayerInteractor player)
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

    void SetOpen(bool shouldOpen)
    {
        bool wasOpen = isOpen;
        isOpen = shouldOpen;

        StopAllCoroutines();
        StartCoroutine(RotateDoor(() =>
        {
            // ONLY when door finishes closing during task
            if (!isOpen && wasOpen && closeDoorTaskActive)
            {
                TasksEvents.OnItemInteract?.Invoke(ItemName.Door);
            }
        }));
    }

    public void Slam()
    {
        StopAllCoroutines();

        isOpen = false;
        transform.rotation = closedRot;
    }

    System.Collections.IEnumerator RotateDoor(System.Action onComplete)
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
        onComplete?.Invoke();
    }
}