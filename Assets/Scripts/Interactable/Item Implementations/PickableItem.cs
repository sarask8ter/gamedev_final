using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickableItem : InteractableAfterEvent, IInteractable
{
    [SerializeField] private ItemName item;
    [SerializeField] private bool useLocalRotationOverride;
    [SerializeField] private Vector3 localRotationOverride;
    [SerializeField] private bool itemForTask;
    [SerializeField] private GameObject pickupSFX;
    [SerializeField] private GameObject placeSFX;
    public ItemName Item => item;

    public override bool IsInteractable => isInteractable && !PlayerInteractor.IsHoldingItem;

    private string oldLayerName;
    private Transform oldParent;

    public override void Interact(PlayerInteractor player)
    {
        if (!player.PickUpItem(this)) return;
        if (pickupSFX != null) Instantiate(pickupSFX);
        oldLayerName = LayerMask.LayerToName(gameObject.layer);
        oldParent = transform.parent;
        MovementHelper.MoveAndDisable(gameObject, player.PickedUpLayerName, player.HoldingPoint, true);
        if (useLocalRotationOverride) gameObject.transform.localRotation = Quaternion.Euler(localRotationOverride);
        isInteractable = false;

        if (itemForTask) TasksEvents.OnItemInteract?.Invoke(item);
    } 

    public void Drop(Transform dropPoint)
    {
        if (oldLayerName == "" ) 
        {
            Debug.LogError("Did not interact previously with object before dropping");
            return;
        }
       if (placeSFX != null) Instantiate(placeSFX);
        transform.SetParent(oldParent);
        MovementHelper.MoveAndEnable(gameObject, oldLayerName, dropPoint, false);
    }
}
