using UnityEngine;

public class RedirectInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactableObject;
    [SerializeField] private bool singleTimeInteract;

    private IInteractable interactable;

    public bool IsInteractable => interactable.IsInteractable;

    void Start()
    {
        interactable = interactableObject.GetComponent<IInteractable>();
    }

    public void Interact(PlayerInteractor player)
    {
        interactable.Interact(player);
        if (singleTimeInteract) MoveAndChangePhysicsMethods.MoveToDefaultLayer(gameObject);
    }
}
