using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Speaker : MonoBehaviour, IInteractable
{
    [SerializeField] protected string npcName;
    [SerializeField] private DialogueNode startNode;

    private DialogueNode currentNode;
    private ProgressEvent lastNodeEvent;

    private bool isTyping;
    private bool isDialogueActive;
    private bool canStartDialogue = true;

    private InteractionPrompt interactionPrompt;
    private InputAction nextLineAction;

    public bool IsInteractable => canStartDialogue && !isDialogueActive;

    void Awake()
    {
        nextLineAction = InputSystem.actions.FindAction("Click");
        interactionPrompt = GetComponentInChildren<InteractionPrompt>();
    }

    void Update()
    {
        if (isDialogueActive && nextLineAction.WasPressedThisFrame())
        {
            if (isTyping)
            {
                StopNode();
                CompleteLine();
            }
            else if (!HasChoices()) GoToNextNode();
        }
    }
    
    void StopNode()
    {
        StopAllCoroutines();
        CancelInvoke();
    }

    public void Interact(PlayerInteractor player)
    {
        if (!canStartDialogue || isDialogueActive) return;

        StartDialogue();
    }

    public void StartDialogue()
    {
        StartDialogue(startNode, npcName);
    }

    public void StartDialogue(DialogueNode node, string speaker)
    {
        isDialogueActive = true;
        lastNodeEvent = ProgressEvent.None;
        currentNode = node;

        if (interactionPrompt != null) interactionPrompt.HideEUI();

        DialogueController.StartDialogue(speaker);
        ShowNode();
    }

    void ShowNode()
    {
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        if (currentNode.TriggeringEvent != ProgressEvent.None) lastNodeEvent = currentNode.TriggeringEvent;

        StopNode();
        DialogueController.ClearChoices();

        StartCoroutine(TypeLine(currentNode.Text));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        DialogueController.SetDialogueText("");

        for (int i = 0; i < line.Length; i++)
        {
            DialogueController.SetDialogueText(line.Substring(0, i + 1));
            yield return new WaitForSeconds(0.02f);
        }

        CompleteLine();
    }

    void CompleteLine()
    {
        DialogueController.SetDialogueText(currentNode.Text);

        isTyping = false;
        
        if (HasChoices())
        {
            ShowChoices();
        }
        else if (currentNode.AutoProgress)
        {
            Invoke(nameof(GoToNextNode), currentNode.AutoDelay);
        }
    }

    void ShowChoices()
    {
        foreach (var choice in currentNode.Choices)
        {
            DialogueController.CreateChoiceButton(choice.Text, () =>
            {
                currentNode = choice.NextNode;
                ShowNode();
            });
        }
    }

    void GoToNextNode()
    {
        currentNode = currentNode.Next;
        ShowNode();
    }

    bool HasChoices()
    {
        return currentNode.Choices != null && currentNode.Choices.Length > 0;
    }

    void EndDialogue()
    {
        StopNode();

        isDialogueActive = false;

        DialogueController.SetDialogueText("");
        DialogueController.ShowDialogueUI(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerStateManager.State = PlayerState.Normal;

        canStartDialogue = false;
        StartCoroutine(ResetDialogueCooldown());

        ProgressManager.CompleteEvent(lastNodeEvent);
    }

    IEnumerator ResetDialogueCooldown()
    {
        yield return new WaitForSeconds(1f);
        canStartDialogue = true;
    }
}