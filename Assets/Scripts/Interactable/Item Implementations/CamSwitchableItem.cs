using UnityEngine;

public class CamSwitchableItem : MonoBehaviour, IInteractable
{
    private bool isInteractable = true;
    public bool IsInteractable => isInteractable;
    [SerializeField] private CinemachineCameraSwitcher camSwitcher;
    [SerializeField] private bool singleTimeInteract;

    public void Interact(PlayerInteractor player)
    {
        camSwitcher.SwitchToNewCam(1);
        if (singleTimeInteract) isInteractable = false;
    }
}
