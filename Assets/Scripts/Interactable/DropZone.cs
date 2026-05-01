using UnityEngine;

public class DropZone : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemName item;
    private bool isActive = true;
    public bool IsInteractable { get => isActive && PlayerInteractor.IsHoldingItem; }

    [SerializeField] private Transform dropPoint;

    public void Interact(PlayerInteractor player)
    {
        if (player.DropHeldItem(item, dropPoint)) {
            isActive = false;
            TasksEvents.OnItemPlace?.Invoke(item);
            gameObject.SetActive(false);
        }
    }
}
