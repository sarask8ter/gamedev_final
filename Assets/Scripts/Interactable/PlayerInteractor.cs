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

    [Header("Hover Indicator")]
    [SerializeField] private float hoverMarkerSize = 14f;
    [SerializeField] private float expandedMarkerSize = 42f;
    [SerializeField] private float hoverExpandSpeed = 10f;

    private PickableItem heldItem;
    private InputAction interactAction;
    private static PlayerInteractor _instance;
    private Texture2D circleTexture;
    private float hoverProgress;
    private bool isHoveringInteractable;
    private Vector2 hoverScreenPosition;

    public static bool IsHoldingItem => _instance.heldItem != null;

    // Returns true if successfully dropped item, false otherwise.
    public bool DropHeldItem(ItemName dropItem, Transform dropPoint)
    {
        if (dropItem != heldItem.Item)
        {
            Debug.LogError("Dropping: " + heldItem + " expected: " + dropItem);
            return false;
        }
        if (heldItem == null) return false;
        heldItem.Drop(dropPoint);
        heldItem = null;
        return true;
    }

    public static bool DiscardItem(ItemName item) { return _instance._DiscardItem(item); }

    // Returns true if successfully discarded item, false otherwise.
    bool _DiscardItem(ItemName item)
    {
        if (item != heldItem.Item)
        {
            Debug.LogError("Discarding: " + heldItem + " expected: " + heldItem.Item);
            return false;
        }
        if (heldItem == null) return false;
        Destroy(heldItem);
        heldItem = null;
        return true;
    }

    // Returns true if holding nothing originally, and now picked up item, false otherwise.
    public bool PickUpItem(PickableItem item)
    {
        Debug.Log("Trying to pick up: " + item.name + " | Currently holding: " + heldItem);

        if (heldItem != null) return false;

        heldItem = item;
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

    void Start()
    {
        // Don't allow held item to preserve between events.
        ProgressManager.OnProgressEventCompleted += (_) => { if (heldItem != null) _DiscardItem(heldItem.Item); };
    }

    void Update()
    {
        UpdateHoverIndicator();

        if ((PlayerStateManager.State == PlayerState.Normal || PlayerStateManager.State == PlayerState.OnlyLookingInput) && interactAction.WasPressedThisFrame())
        {
            TryInteract();
        }
    }

    void OnDestroy()
    {
        if (circleTexture != null)
        {
            Destroy(circleTexture);
        }
    }

    void UpdateHoverIndicator()
    {
        isHoveringInteractable = false;

        if (PlayerStateManager.State == PlayerState.Normal || PlayerStateManager.State == PlayerState.OnlyLookingInput)
        {
            if (TryGetTargetInteractable(out IInteractable interactable, out RaycastHit hit) &&
                interactable.IsInteractable &&
                ShouldShowHoverIndicator(interactable))
            {
                Vector3 markerWorldPosition = hit.point + hit.normal * 0.03f;
                Vector3 markerScreenPosition = cam.WorldToScreenPoint(markerWorldPosition);

                if (markerScreenPosition.z > 0f)
                {
                    isHoveringInteractable = true;
                    hoverScreenPosition = new Vector2(markerScreenPosition.x, Screen.height - markerScreenPosition.y);
                }
            }
        }

        float targetProgress = isHoveringInteractable ? 1f : 0f;
        hoverProgress = Mathf.MoveTowards(hoverProgress, targetProgress, hoverExpandSpeed * Time.deltaTime);
    }

    bool ShouldShowHoverIndicator(IInteractable interactable)
    {
        return interactable is Inspectable ||
               interactable is PickableItem ||
               interactable is MovableItem;
    }

    void OnGUI()
    {
        if (hoverProgress <= 0f) return;

        if (circleTexture == null)
        {
            circleTexture = CreateCircleTexture(64);
        }

        Color previousColor = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, hoverProgress);

        float markerSize = Mathf.Lerp(hoverMarkerSize, expandedMarkerSize, hoverProgress);
        Rect markerRect = new Rect(
            hoverScreenPosition.x - markerSize * 0.5f,
            hoverScreenPosition.y - markerSize * 0.5f,
            markerSize,
            markerSize
        );
        GUI.DrawTexture(markerRect, circleTexture);

        GUI.color = previousColor;
    }

    Texture2D CreateCircleTexture(int size)
    {
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        float radius = (size - 1) * 0.5f;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                float alpha = Mathf.Clamp01(radius - distance);
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }

        texture.Apply();
        return texture;
    }

    void TryInteract()
    {
        Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * raycastDist, Color.red, 1f);

        if (TryGetTargetInteractable(out IInteractable interactable, out RaycastHit hit))
        {
            Debug.Log("Hit: " + hit.collider.name);
            Debug.Log("Found interactable");

            if (interactable.IsInteractable)
            {
                Debug.Log("Interacting");
                interactable.Interact(this);
            }
        }
    }

    bool TryGetTargetInteractable(out IInteractable interactable, out RaycastHit hit)
    {
        interactable = null;

        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, raycastDist, interactableMask))
        {
            return false;
        }

        interactable = hit.collider.GetComponentInParent<IInteractable>();
        return interactable != null;
    }
}
