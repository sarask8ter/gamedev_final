using System.Linq;
using UnityEngine;

public class ChangeUIOnInteractableHover : MonoBehaviour
{
    [SerializeField] private GameObject hoverElement;
    [SerializeField] private PlayerState[] showHoverStates;

    void Update()
    {
        if (!showHoverStates.Contains(PlayerStateManager.State)) {
            hoverElement.SetActive(false);
            return;
        };

        var interactable = PlayerInteractor.RaycastInteractable();
        var isHoveringInteractable = interactable != null && interactable.IsInteractable;
        hoverElement.SetActive(isHoveringInteractable);
    }
}