using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TransformableItem : MonoBehaviour, IInteractable
{
    private bool isInteractable;
    public bool IsInteractable => isInteractable;
    [SerializeField] private ItemName item;
    [SerializeField] private Mesh originalMesh;
    [SerializeField] private Mesh newMesh;
    [SerializeField] private ProgressEvent unlockEvent;
    private MeshFilter meshFilter;
    private bool isTransformed;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        ProgressManager.SubscribeToStart(unlockEvent, () => isInteractable = true);
        ProgressManager.SubscribeToEnd(unlockEvent, () => isInteractable = false);
    }

    public void Interact(PlayerInteractor player)
    {
        meshFilter.mesh = isTransformed ? originalMesh : newMesh;
        isTransformed = !isTransformed;
        TasksEvents.OnItemInteract?.Invoke(item);
    }
}
