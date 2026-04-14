using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Neighbor : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;
    private bool canStartDialogue = true;
    public bool IsInteractable => canStartDialogue;
    
    private InteractionPrompt interactionPrompt;

    private InputAction nextLineAction;

    void Awake()
    {
        nextLineAction = InputSystem.actions.FindAction("Click");
        interactionPrompt = GetComponentInChildren<InteractionPrompt>();
    }

    void Start()
    {
        dialogueUI = DialogueController.Instance;
    }

    void Update()
    {
        if (isDialogueActive && nextLineAction.WasPressedThisFrame()) NextLine();
    }

    public void Interact(PlayerInteractor player)
    {
        if (dialogueData == null) return;

        if (canStartDialogue && !isDialogueActive)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        PlayerStateManager.State = PlayerState.Dialogue;
        interactionPrompt.HideEUI();

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

        if (GetDialogueChoice() != null) return;

        dialogueUI.ClearChoices();

        dialogueIndex++;

        if (dialogueIndex >= dialogueData.dialogueLines.Length ||
            (dialogueData.endDialogueLines != null &&
            dialogueIndex < dialogueData.endDialogueLines.Length &&
            dialogueData.endDialogueLines[dialogueIndex]))
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

        if (dialogueData.endDialogueLines != null &&
            dialogueIndex < dialogueData.endDialogueLines.Length &&
            dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            yield break;
        }

        TryShowChoicesOrContinue();

        // FORCE FLOW CONTINUATION HERE
        TryShowChoicesOrContinue();
    }

    void DisplayCurrentLine()
    {
        CancelAllAutoDialogue();
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
        CancelInvoke(nameof(NextLine));

        if (nextIndex < 0 || nextIndex >= dialogueData.dialogueLines.Length)
        {
            Debug.LogWarning("Invalid nextIndex: " + nextIndex);
            EndDialogue();
            return;
        }

        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }

    public void EndDialogue()
    {
        PlayerStateManager.State = PlayerState.Normal;
        
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

        dialogueUI.ClearChoices(); // ✅ MOVE THIS TO THE TOP

        var choice = GetDialogueChoice();
        if (choice != null) DisplayChoices(choice);

        if (dialogueData.autoProgressLines != null &&
            dialogueIndex < dialogueData.autoProgressLines.Length &&
            dialogueData.autoProgressLines[dialogueIndex])
        {
            Invoke(nameof(NextLine), dialogueData.autoProgressDelay);
        }
    }

    DialogueChoice GetDialogueChoice()
    {
        if (dialogueData.choices != null)
        {
            foreach (DialogueChoice dialogueChoice in dialogueData.choices)
            {
                if (dialogueChoice.dialogueIndex == dialogueIndex)
                {
                    return dialogueChoice;
                }
            }
        }

        return null;
    }

    void CancelAllAutoDialogue()
    {
        CancelInvoke(nameof(NextLine));
    }
}