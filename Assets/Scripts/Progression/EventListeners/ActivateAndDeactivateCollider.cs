using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ActivateAndDeactivateCollider : MonoBehaviour
{
    [SerializeField] private ProgressEvent activateEvent;
    [SerializeField] private ProgressEvent deactivateEvent;
    private Collider col;

    void Awake()
    {
       col = GetComponent<Collider>();
    }

    void Start()
    {
        ProgressManager.SubscribeToStart(activateEvent, () => col.enabled = true);
        ProgressManager.SubscribeToStart(deactivateEvent, () => col.enabled = false);
    }
}