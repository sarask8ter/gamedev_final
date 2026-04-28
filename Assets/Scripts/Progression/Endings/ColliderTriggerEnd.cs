using UnityEngine;

public class ColliderTriggerEnd : MonoBehaviour
{
    [SerializeField] private DialogueNode leaveNoEvidenceDialogue;
    [SerializeField] private DialogueNode leaveWithEvidenceDialogue;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var lastDialogue = leaveNoEvidenceDialogue;
            if (EndingState.ChosenEnding == Ending.LeaveAndCallPolice) lastDialogue = leaveWithEvidenceDialogue;
            DialogueManager.StartDialogue(lastDialogue);
        }
    }
}