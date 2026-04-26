using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueNode[] dialogues;
    private DialogueNode currentNode;
    private ProgressEvent lastNodeEvent;
    private bool isTyping;
    private bool isDialogueActive;
    private InputAction nextLineAction;

    void Awake()
    {
        nextLineAction = InputSystem.actions.FindAction("Click");
    }

    void Start()
    {
        // No need to unsubscribe -- ProgressManager handles that.
        foreach (var dialogue in dialogues) ProgressManager.SubscribeToStart(dialogue.TriggeringEvent, () => StartDialogue(dialogue));
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
            else if (!HasChoices() && currentNode.AllowClickToNextNode) GoToNextNode();
        }
    }
    
    void StopNode()
    {
        StopAllCoroutines();
        CancelInvoke();
    }

    public void StartDialogue(DialogueNode node)
    {
        isDialogueActive = true;
        lastNodeEvent = ProgressEvent.None;
        currentNode = node;
        DialogueUIController.StartDialogue();
        ShowNode();
    }

    void ShowNode()
    {
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        DialogueUIController.SetNPCInfo(currentNode.SpeakerName);

        if (currentNode.TriggeringEvent != ProgressEvent.None) lastNodeEvent = currentNode.TriggeringEvent;

        StopNode();
        DialogueUIController.ClearChoices();

        StartCoroutine(TypeLine(currentNode.Text));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        DialogueUIController.SetDialogueText("");

        for (int i = 0; i < line.Length; i++)
        {
            DialogueUIController.SetDialogueText(line.Substring(0, i + 1));
            yield return new WaitForSeconds(0.02f);
        }

        CompleteLine();
    }

    void CompleteLine()
    {
        DialogueUIController.SetDialogueText(currentNode.Text);

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
            DialogueUIController.CreateChoiceButton(choice.Text, () =>
            {
                HandleChoice(choice);
            });
        }
    }

    void HandleChoice(DialogueChoice choice)
    {
        // 1. run gameplay logic
        choice.OnChooseChoice?.Execute();

        // 2. move dialogue forward
        currentNode = choice.NextNode;
        ShowNode();
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

        DialogueUIController.SetDialogueText("");
        DialogueUIController.ShowDialogueUI(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerStateManager.State = PlayerState.Normal;
        ProgressManager.CompleteEvent(lastNodeEvent);
    }
}