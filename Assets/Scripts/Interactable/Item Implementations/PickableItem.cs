using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ProgressEvent unlockEvent;
    [SerializeField] private ItemName item;
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
        MoveAndChangePhysicsMethods.MoveAndDisable(gameObject, player.PickedUpLayerName, player.HoldingPoint, true);
        isInteractable = false;
    }

    public void Drop(Transform dropPoint)
    {
        if (oldLayerName == "" ) 
        {
            Debug.LogError("Did not interact previously with object before dropping");
            return;
        }
        transform.SetParent(oldParent);
        MoveAndChangePhysicsMethods.MoveAndEnable(gameObject, oldLayerName, dropPoint, false);
    }
}
