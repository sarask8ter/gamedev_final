using UnityEngine;

public class Inspectable : MonoBehaviour, IInteractable
{
    public bool IsInteractable => PlayerStateManager.State == PlayerState.Normal && !PlayerInteractor.IsHoldingItem;
    [SerializeField] private GameObject inspectPrefab;
    [SerializeField] private float inspectScale = 1;
    [SerializeField] private DialogueNode dialogue;
    [SerializeField] private bool isEvidence;

    public void Interact(PlayerInteractor player)
    {
        var inspectClone = Instantiate(inspectPrefab != null ? inspectPrefab : gameObject);
        inspectClone.transform.localScale *= inspectScale;
        PlayerInspector.BeginInspection(inspectClone);
        if (dialogue != null) DialogueManager.StartDialogue(dialogue, false, false);
        if (isEvidence) GameState.FoundEvidence = true;
    }
}
