using UnityEngine;

public class Inspectable : InteractableAfterEvent, IInteractable
{
    public override bool IsInteractable => isInteractable && PlayerStateManager.State == PlayerState.Normal && !PlayerInteractor.IsHoldingItem;
    [SerializeField] private GameObject inspectPrefab;
    [SerializeField] private float inspectScale = 1;
    [SerializeField] private bool useLocalRotationOverride;
    [SerializeField] private Vector3 localRotationOverride;
    [SerializeField] private DialogueNode dialogue;
    [SerializeField] private bool isEvidence;
    [SerializeField] private bool turnOffInteractableAfterEventEnd = true;

    public override void Interact(PlayerInteractor player)
    {
        var inspectClone = Instantiate(inspectPrefab != null ? inspectPrefab : gameObject);
        inspectClone.transform.localScale *= inspectScale;
        PlayerInspector.BeginInspection(inspectClone);
        if (useLocalRotationOverride) inspectClone.transform.localRotation = Quaternion.Euler(localRotationOverride);
        if (dialogue != null) DialogueManager.StartDialogue(dialogue, false, false);
        if (isEvidence) GameState.FoundEvidence = true;
    }

    protected override void PostStart()
    {
        if (turnOffInteractableAfterEventEnd) ProgressManager.SubscribeToEnd(unlockEvent, () => isInteractable = false);
    }
}
