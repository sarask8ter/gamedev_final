using UnityEngine;

public class DropZone : MonoBehaviour, IInteractable
{
    private string itemId = "box";
    private bool isIteractable = true;
    public bool IsInteractable { get => isIteractable; }

    [SerializeField] private Transform dropPoint;

    public void Interact(PlayerInteractor player)
    {
        if (player.DropHeldItem(itemId, dropPoint)) isIteractable = false;
    }
}
