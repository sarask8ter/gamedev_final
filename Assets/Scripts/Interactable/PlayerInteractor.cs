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

    private Pickable heldItem;
    private InputAction interactAction;
    private static PlayerInteractor _instance;

    public static bool IsHoldingItem => _instance.heldItem != null;

    // Returns true if successfully dropped item, false otherwise.
    public bool DropHeldItem(ItemName dropItem, Transform dropPoint)
    {
        Debug.Log("Dropping: " + heldItem + " expected: " + dropItem);
        if (heldItem == null) return false;
        heldItem.Drop(dropPoint);
        heldItem = null;
        return true;
    }

    // Returns true if holding nothing originally, and now picked up item, false otherwise.
    public bool PickUpItem(Pickable item)
    {
        Debug.Log("Trying to pick up: " + item.name + " | Currently holding: " + heldItem);

        if (heldItem != null) return false;

        heldItem = item;
        return true;
    }

    void Awake()
    {
        interactAction = InputSystem.actions.FindAction("Interact");

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
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
        Debug.Log("Trying interact");

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, raycastDist, interactableMask))
        {
            Debug.Log("Hit: " + hit.collider.name);

            var interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                Debug.Log("Found interactable");

                if (interactable.IsInteractable)
                {
                    Debug.Log("Interacting");
                    interactable.Interact(this);
                }
            }
        }
    }
}
