using UnityEngine;

public class Inspectable : MonoBehaviour, IInteractable
{
    public bool IsInteractable => PlayerStateManager.State == PlayerState.Normal && !PlayerInteractor.IsHoldingItem;

    public void Interact(PlayerInteractor player)
    {
        PlayerInspector.BeginInspection(gameObject);
    }
}
