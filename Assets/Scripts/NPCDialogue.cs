using UnityEngine;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    public DialogueSystem dialogueSystem;

    public void Interact()
    {
        StartCoroutine(DialogueFlow());
    }

    IEnumerator DialogueFlow()
    {
        dialogueSystem.StartDialogue("Jerry", new string[]
        {
            "Hey... something feels off.",
            "Can you help me check outside?"
        });

        dialogueSystem.ShowChoices(
            "I'll help.",
            "Leave me alone.",
            "(inner thoughts this guy is creepy but could be useful later on) I'm good thanks",
            OnChoiceMade
        );
    }

    void OnChoiceMade(int choice)
    {
        if (choice == 1)
        {
            dialogueSystem.StartDialogue("You", new string[]
            {
                "I trust you."
            });

            StartCoroutine(NeighborPath());
        }
        else if (choice == 2)
        {
            dialogueSystem.StartDialogue("You", new string[]
            {
                "...I hear it too."
            });

            StartCoroutine(PresencePath());
        }
        else if (choice == 3)
        {
            dialogueSystem.StartDialogue("You", new string[]
            {
                "I'm ignoring this."
            });

            StartCoroutine(IgnorePath());
        }
    }

    IEnumerator NeighborPath()
    {
        yield return new WaitForSeconds(2f);

        dialogueSystem.StartDialogue("Jerry", new string[]
        {
            "Good... stay close."
        });
    }

    IEnumerator PresencePath()
    {
        yield return new WaitForSeconds(2f);

        dialogueSystem.StartDialogue("???", new string[]
        {
            "You made the right choice..."
        });
    }

    IEnumerator IgnorePath()
    {
        yield return new WaitForSeconds(2f);

        dialogueSystem.StartDialogue("Jerry", new string[]
        {
            "...you shouldn’t have done that."
        });
    }
}