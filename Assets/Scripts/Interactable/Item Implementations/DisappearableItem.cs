using UnityEngine;

public class DisappearableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemName item;
    public ItemName Item => item;

    private bool isInteractable = true;
    public bool IsInteractable => isInteractable;

    public void Interact(PlayerInteractor player)
    {
        TasksEvents.OnItemInteract?.Invoke(item);
        gameObject.SetActive(false);
    }
}
