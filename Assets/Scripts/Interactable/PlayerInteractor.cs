using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private Transform holdingPoint;
    public Transform HoldingPoint => holdingPoint;

    [SerializeField] private float raycastDist;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private string pickedUpLayerName;
    public string PickedUpLayerName => pickedUpLayerName;

    [SerializeField] private PickableItem heldItem;
    private InputAction interactAction;
    private static PlayerInteractor _instance;

    public static bool IsHoldingItem => _instance.heldItem != null;

    // Returns true if successfully dropped item, false otherwise.
    public bool DropHeldItem(ItemName dropItem, Transform dropPoint)
    {
        if (heldItem == null) return false;
        if (dropItem != heldItem.Item)
        {
            Debug.LogError("Dropping: " + heldItem + " expected: " + dropItem);
            return false;
        }
        heldItem.Drop(dropPoint);
        heldItem = null;
        return true;
    }

    public static bool DiscardItem(ItemName item) { return _instance._DiscardItem(item); }

    public static IInteractable RaycastInteractable()
    {
        return _instance._RaycastInteractable();
    }

    IInteractable _RaycastInteractable()
    {
        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, raycastDist, interactableMask))
            return null;
        Debug.Log("Hit: " + hit.collider.name);
        var interactable = hit.collider.GetComponentInParent<IInteractable>();
        return interactable;
    }

    // Returns true if successfully discarded item, false otherwise.
    bool _DiscardItem(ItemName item)
    {
        if (heldItem == null) {
            Debug.LogError("Trying to discard nothing");
            return false;
        }

        if (item != heldItem.Item)
        {
            Debug.LogError("Discarding: " + heldItem + " expected: " + heldItem.Item);
            return false;
        }

        Debug.Log("Destroying " + heldItem.Item);
        Destroy(heldItem.gameObject);
        heldItem = null;
        return true;
    }

    // Returns true if holding nothing originally, and now picked up item, false otherwise.
    public bool PickUpItem(PickableItem item)
    {
        if (heldItem != null) {
            Debug.Log("Trying to pick up: " + item.name + " | Currently holding: " + heldItem);
            return false;
        }

        heldItem = item;
        Debug.Log("Picked up " + item);
        return true;
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            interactAction = InputSystem.actions.FindAction("Interact");
        }
    }

    void Update()
    {
        if ((PlayerStateManager.State == PlayerState.Normal || PlayerStateManager.State == PlayerState.OnlyLookingInput) && interactAction.WasPressedThisFrame())
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * raycastDist, Color.red, 1f);
        var interactable = _RaycastInteractable();

        if (interactable != null)
        {
            Debug.Log("Found interactable " + interactable + " can interact? " + interactable.IsInteractable);

            if (interactable.IsInteractable)
            {
                Debug.Log("Interacting");
                interactable.Interact(this);
            }
        }
    }
}
