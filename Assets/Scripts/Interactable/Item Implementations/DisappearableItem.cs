using UnityEngine;

public class DisappearableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemName item;
    public ItemName Item => item;

    private bool isIteractable = true;
    public bool IsInteractable => isIteractable;

    public void Interact(PlayerInteractor player)
    {
        TasksEvents.OnItemInteract?.Invoke(item);
        gameObject.SetActive(false);
    }
}
