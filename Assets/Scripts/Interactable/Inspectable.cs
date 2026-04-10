using UnityEngine;

public class Inspectable : MonoBehaviour, IInteractable
{
    public bool IsInteractable => PlayerStateManager.State == PlayerState.Normal;

    public void Interact(PlayerInteractor player)
    {
        PlayerInspector.BeginInspection(gameObject);
    }
}
