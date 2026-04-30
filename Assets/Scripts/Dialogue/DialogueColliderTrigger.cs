using UnityEngine;

public class DialogueColliderTrigger : MonoBehaviour
{
    [SerializeField] private DialogueNode dialogue;
    [SerializeField] private ProgressEvent unlockEvent;
    private bool unlocked;

    void Start()
    {
        ProgressManager.SubscribeToStart(unlockEvent, () => unlocked = true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (unlocked && other.gameObject.CompareTag("Player"))
        {
            DialogueManager.StartDialogue(dialogue);
            PostTrigger();
            unlocked = false;
            gameObject.SetActive(false);
        }
    }

    protected virtual void PostTrigger() {}
}