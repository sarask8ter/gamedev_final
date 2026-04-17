using UnityEngine;

public class MovableItem : MonoBehaviour, IInteractable
{
    private bool isInteractable;
    public bool IsInteractable => isInteractable;
    [SerializeField] private ItemName item;
    [SerializeField] private ProgressEvent unlockEvent;

    [SerializeField] private Transform originalLocation;
    [SerializeField] private Transform movedLocation;
    [SerializeField] private bool singleTimeInteract;

    private bool isMoved;

    void Start()
    {
        ProgressManager.SubscribeToStart(unlockEvent, () => isInteractable = true);
        // ProgressManager.SubscribeToEnd(unlockEvent, () => isInteractable = false);
    }

    public void Interact(PlayerInteractor player)
    {
        MoveAndChangePhysicsMethods.MoveToPoint(gameObject, isMoved ? originalLocation : movedLocation, false);
        isMoved = !isMoved;
        TasksEvents.OnItemInteract?.Invoke(item);

        if (singleTimeInteract) 
        {
            MoveAndChangePhysicsMethods.MoveToDefaultLayer(gameObject);
            isInteractable = false;
        }
    }
}
