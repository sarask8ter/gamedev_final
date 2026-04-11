using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Neighbor : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;
    private bool canStartDialogue = true;

    private void Start()
    {
        dialogueUI = DialogueController.Instance;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canStartDialogue)
        {
            if (!isDialogueActive)
            {
                StartDialogue();
            }
        }
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null) return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        Debug.Log(dialogueData);
        Debug.Log("START DIALOGUE");

        isDialogueActive = true;
        dialogueIndex = 0;

        dialogueUI.ShowDialogueUI(true);
        dialogueUI.SetNPCInfo(dialogueData.npcName);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DisplayCurrentLine();
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            isTyping = false;

            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            TryShowChoicesOrContinue();
            return;
        }

        dialogueUI.ClearChoices();

        dialogueIndex++;

        if (dialogueIndex >= dialogueData.dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        DisplayCurrentLine();
    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        string line = dialogueData.dialogueLines[dialogueIndex];
        dialogueUI.SetDialogueText("");

        Debug.Log("TYPELINE STARTED");

        for (int i = 0; i < line.Length; i++)
        {
            dialogueUI.SetDialogueText(line.Substring(0, i + 1));
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        // FORCE FLOW CONTINUATION HERE
        TryShowChoicesOrContinue();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    void DisplayChoices(DialogueChoice choice)
    {
        Debug.Log("DISPLAY CHOICES CALLED");

        if (choice.choices == null || choice.nextDialogueIndexes == null)
        {
            Debug.LogError("Choice arrays are NULL");
            return;
        }

        int count = Mathf.Min(choice.choices.Length, choice.nextDialogueIndexes.Length);

        for (int i = 0; i < count; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            string text = choice.choices[i];

            dialogueUI.CreateChoiceButton(text, () => ChooseOption(nextIndex));
        }
    }

    void ChooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;

        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canStartDialogue = false;
        StartCoroutine(ResetDialogueCooldown());
    }

    IEnumerator ResetDialogueCooldown()
    {
        yield return new WaitForSeconds(1f);
        canStartDialogue = true;
    }

    void TryShowChoicesOrContinue()
    {
        Debug.Log("CHECKING CHOICES AT INDEX: " + dialogueIndex);

        if (dialogueIndex == 1)
        {
            Debug.Log("FORCING TEST CHOICE");
            DisplayChoices(dialogueData.choices[0]);
        }


        dialogueUI.ClearChoices();

        if (dialogueData.choices != null)
        {
            foreach (DialogueChoice dialogueChoice in dialogueData.choices)
            {
                if (dialogueChoice.dialogueIndex == dialogueIndex)
                {
                    DisplayChoices(dialogueChoice);
                    return;
                }
            }
        }

        if (dialogueData.autoProgressLines != null &&
            dialogueIndex < dialogueData.autoProgressLines.Length &&
            dialogueData.autoProgressLines[dialogueIndex])
        {
            Invoke(nameof(NextLine), dialogueData.autoProgressDelay);
        }
    }
}