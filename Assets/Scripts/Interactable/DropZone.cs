using UnityEngine;

public class DropZone : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemName item;
    private bool isInteractable = true;
    public bool IsInteractable { get => isInteractable; }

    [SerializeField] private Transform dropPoint;

    public void Interact(PlayerInteractor player)
    {
        if (player.DropHeldItem(item, dropPoint)) {
            isInteractable = false;
            TasksEvents.OnItemPlace?.Invoke(item);
            gameObject.SetActive(false);
        }
    }
}
