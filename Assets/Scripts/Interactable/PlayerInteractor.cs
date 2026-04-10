using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private Transform holdingPoint;
    public Transform HoldingPoint => holdingPoint;

    [SerializeField] private float raycastDist;
    [SerializeField] private LayerMask interactableMask;

    public Pickable heldItem;
    private InputAction interactAction;

    // Returns true if successfully dropped item, false otherwise.
    public bool DropHeldItem(ItemName dropItem, Transform dropPoint)
    {
        if (heldItem == null || heldItem.Item != dropItem) return false;
        heldItem.Drop(dropPoint);
        heldItem = null;
        return true;
    }

    // Returns true if holding nothing originally, and now picked up item, false otherwise.
    public bool PickUpItem(Pickable item)
    {
        if (heldItem != null) return false;
        heldItem = item;
        return true;
    }

    void Awake()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (PlayerStateManager.State == PlayerState.Normal && interactAction.WasPressedThisFrame())
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * raycastDist, Color.red, 2f);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, raycastDist, interactableMask))
        {
            var interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null && interactable.IsInteractable)
            {
                interactable.Interact(this);
            }
        }
    }
}
