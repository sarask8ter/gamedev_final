using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ProgressEvent unlockEvent;
    [SerializeField] private ItemName item;
    [SerializeField] private bool useLocalRotationOverride;
    [SerializeField] private Vector3 localRotationOverride;
    [SerializeField] private bool itemForTask;
    public ItemName Item => item;

    private bool isInteractable;
    public bool IsInteractable => isInteractable && !PlayerInteractor.IsHoldingItem;

    private string oldLayerName;
    private Transform oldParent;

    void Start()
    {
        ProgressManager.SubscribeToStart(unlockEvent, () => isInteractable = true);
    }

    public void Interact(PlayerInteractor player)
    {
        if (!player.PickUpItem(this)) return;
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
        transform.SetParent(oldParent);
        MovementHelper.MoveAndEnable(gameObject, oldLayerName, dropPoint, false);
    }
}
