using UnityEngine;

public class DialogueColliderTrigger : EventAction
{
    [SerializeField] private DialogueNode dialogue;
    private bool unlocked;

    public override void OnEventStart()
    {
        unlocked = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (unlocked && other.gameObject.CompareTag("Player"))
        {
            DialogueManager.StartDialogue(dialogue);
            unlocked = false;
            gameObject.SetActive(false);
        }
    }
}