using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickable : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemName item;
    public ItemName Item => item;

    private bool isIteractable = true;
    public bool IsInteractable => isIteractable;

    private Collider col;
    private string oldLayerName;

    void Awake()
    {
        col = GetComponent<Collider>();
    }

    public void Interact(PlayerInteractor player)
    {
        if (!player.PickUpItem(this)) return;
        oldLayerName = LayerMask.LayerToName(gameObject.layer);
        MoveAndChangePhysicsMethods.MoveAndDisable(gameObject, player.PickedUpLayerName, player.HoldingPoint);
        isIteractable = false;
    }

    public void Drop(Transform dropPoint)
    {
        if (oldLayerName == "") throw new System.Exception("Did not interact previously with object before dropping");
        MoveAndChangePhysicsMethods.MoveAndEnable(gameObject, oldLayerName, dropPoint);
    }
}
