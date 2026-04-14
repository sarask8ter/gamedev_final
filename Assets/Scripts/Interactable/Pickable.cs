using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickable : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemName item;
    public ItemName Item => item;

    private bool isIteractable = true;
    public bool IsInteractable => isIteractable;

    private string oldLayerName;
    private Transform oldParent;

    public void Interact(PlayerInteractor player)
    {
        if (!player.PickUpItem(this)) return;
        oldLayerName = LayerMask.LayerToName(gameObject.layer);
        oldParent = transform.parent;
        MoveAndChangePhysicsMethods.MoveAndDisable(gameObject, player.PickedUpLayerName, player.HoldingPoint, true);
        isIteractable = false;
    }

    public void Drop(Transform dropPoint)
    {
        if (oldLayerName == "" ) throw new System.Exception("Did not interact previously with object before dropping");
        transform.SetParent(oldParent);
        MoveAndChangePhysicsMethods.MoveAndEnable(gameObject, oldLayerName, dropPoint, false);
    }
}
