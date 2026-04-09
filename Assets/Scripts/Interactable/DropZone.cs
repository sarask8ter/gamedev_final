using UnityEngine;

public class DropZone : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemName item;
    private bool isIteractable = true;
    public bool IsInteractable { get => isIteractable; }

    [SerializeField] private Transform dropPoint;

    public void Interact(PlayerInteractor player)
    {
        if (player.DropHeldItem(item, dropPoint)) {
            isIteractable = false;
            TasksEvents.OnItemPlace?.Invoke(item);
            gameObject.SetActive(false);
        }
    }
}
