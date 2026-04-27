using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    private DialogueNode currentNode;
    private ProgressEvent lastNodeEvent;
    private bool isTyping;
    private bool isDialogueActive;
    private InputAction nextLineAction;
    private static DialogueManager _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            nextLineAction = InputSystem.actions.FindAction("Click");
        }
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

    public static void StartDialogue(DialogueNode node)
    {
        _instance.isDialogueActive = true;
        _instance.lastNodeEvent = ProgressEvent.None;
        _instance.currentNode = node;
        DialogueUIController.StartDialogue();
        _instance.ShowNode();
    }

    void ShowNode()
    {
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        DialogueUIController.SetNPCInfo(currentNode.speaker.GetName());

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