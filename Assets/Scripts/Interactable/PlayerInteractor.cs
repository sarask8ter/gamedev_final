using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private Transform holdingPoint;
    public Transform HoldingPoint => holdingPoint;

    [SerializeField] private float raycastDist;
    [SerializeField] private LayerMask interactableMask;

    private Pickable heldItem;
    private InputAction interactAction;

    // Returns true if successfully dropped item, false otherwise.
    public bool DropHeldItem(string dropItemId, Transform dropPoint)
    {
        if (heldItem == null || heldItem.Id != dropItemId) return false;
        heldItem.Drop(dropPoint);
        heldItem = null;
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
            var inspectable = hit.collider.GetComponentInParent<Inspectable>();

            if (interactable != null && interactable.IsInteractable)
            {
                interactable.Interact(this);
                if (interactable is Pickable pickableObj)
                {
                    heldItem = pickableObj;
                }
            }
            else if (inspectable != null)
            {
                PlayerInspector.BeginInspection(hit.collider.gameObject);
            }
        }
    }
}
