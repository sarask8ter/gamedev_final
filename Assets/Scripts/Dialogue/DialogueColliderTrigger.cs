using UnityEngine;

public class DialogueColliderTrigger : MonoBehaviour
{
    [SerializeField] private DialogueNode dialogue;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DialogueManager.StartDialogue(dialogue);
            PostTrigger();
            gameObject.SetActive(false);
        }
    }

    protected virtual void PostTrigger() {}
}