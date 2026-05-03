using System.Linq;
using UnityEngine;

public class ChangeUIOnInteractableHover : MonoBehaviour
{
    [SerializeField] private GameObject hoverElement;
    [SerializeField] private PlayerState[] showHoverStates;
    [SerializeField] private GameObject interactInstructionsUI;

    void Update()
    {
        if (!showHoverStates.Contains(PlayerStateManager.State)) {
            hoverElement.SetActive(false);
            interactInstructionsUI.SetActive(false);
            return;
        };

        var interactable = PlayerInteractor.RaycastInteractable();
        var isHoveringInteractable = interactable != null && interactable.IsInteractable;
        hoverElement.SetActive(isHoveringInteractable);
        interactInstructionsUI.SetActive(isHoveringInteractable);
    }
}