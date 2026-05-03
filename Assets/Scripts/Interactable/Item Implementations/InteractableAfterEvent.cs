using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteractableAfterEvent : MonoBehaviour, IInteractable
{
    [SerializeField] protected ProgressEvent unlockEvent;
    public virtual bool IsInteractable => isInteractable;
    protected bool isInteractable;

    void Start()
    {
        ProgressManager.SubscribeToStart(unlockEvent, () => isInteractable = true);
        PostStart();
    }

    public abstract void Interact(PlayerInteractor player);

    protected virtual void PostStart() {}
}
