using UnityEngine;

public class PlayerSpeaker : Speaker
{
    [SerializeField] private DialogueNode[] dialogues;

    void Start()
    {
        // No need to unsubscribe -- ProgressManager handles that.
        foreach (var dialogue in dialogues) ProgressManager.SubscribeToStart(dialogue.TriggeringEvent, () => StartDialogue(dialogue, npcName));
    }
}
