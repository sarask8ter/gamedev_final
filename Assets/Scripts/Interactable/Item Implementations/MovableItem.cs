using UnityEngine;

public class MovableItem : InteractableAfterEvent, IInteractable
{
    [SerializeField] private ItemName item;
    [SerializeField] private Transform originalLocation;
    [SerializeField] private Transform movedLocation;
    [SerializeField] private bool singleTimeInteract;

    private bool isMoved;

    public override void Interact(PlayerInteractor player)
    {
        MovementHelper.MoveToPoint(gameObject, isMoved ? originalLocation : movedLocation, false);
        isMoved = !isMoved;
        TasksEvents.OnItemInteract?.Invoke(item);

        if (singleTimeInteract) 
        {
            MovementHelper.MoveToDefaultLayer(gameObject);
            isInteractable = false;
        }
    }
}
