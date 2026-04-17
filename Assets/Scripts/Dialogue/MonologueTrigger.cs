using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueNode startNode;

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            Speaker neighbor = FindAnyObjectByType<Speaker>();
            neighbor.StartDialogue(startNode, ""); // empty name = monologue
        }
    }
}