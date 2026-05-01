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
    private bool changePlayerState;
    private bool isEndingDialogue;
    private Coroutine autoProgressCoroutine;

    [SerializeField] private AudioSource typingAudioSource;
    [SerializeField] private AudioClip[] typingClips;

    public static void CancelDialogue()
    {
        if (_instance == null) return;
        if (!_instance.isDialogueActive) return;

        _instance.EndDialogue(false);
    }

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
        if (currentNode == null) return;

        if (currentNode.AllowClickToNextNode && isDialogueActive && nextLineAction.WasPressedThisFrame())
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
        StopTypingSound();

        if (autoProgressCoroutine != null)
        {
            // Owned by CoroutineHelper so need to manually cancel.
            CoroutineHelper.Cancel(autoProgressCoroutine);
            autoProgressCoroutine = null;
        }
    }

    public static void StartDialogue(DialogueNode node, bool lockCursor = true, bool changePlayerState = true)
    {
        _instance.isDialogueActive = true;
        _instance.lastNodeEvent = ProgressEvent.None;
        _instance.currentNode = node;
        DialogueUIController.StartDialogue(lockCursor);
        _instance.changePlayerState = changePlayerState;
        _instance.isEndingDialogue = node.IsEndingDialogue;
        if (changePlayerState) PlayerStateManager.State = PlayerState.Dialogue;
        _instance.ShowNode();
    }

    void ShowNode()
    {
        if (currentNode == null)
        {
            EndDialogue(true);
            return;
        }

        if (currentNode.IsEndingDialogue) isEndingDialogue = true;

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

        PlayTypingSound();

        for (int i = 0; i < line.Length; i++)
        {
            DialogueUIController.SetDialogueText(line.Substring(0, i + 1));
            yield return new WaitForSeconds(0.02f);
        }

        StopTypingSound();

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
            var nodeToProgress = currentNode;
            autoProgressCoroutine = CoroutineHelper.Delay(currentNode.AutoDelay, () =>
            {
                autoProgressCoroutine = null;

                if (isDialogueActive && nodeToProgress == currentNode)
                {
                    GoToNextNode();
                }
            });
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
        foreach (var action in choice.OnChooseChoice) action.Execute();

        // 2. move dialogue forward
        currentNode = choice.NextNode;
        ShowNode();
    }

    void GoToNextNode()
    {
        if (currentNode == null) return;
        var nextNode = currentNode.Next;
        if (currentNode.NextIfFoundEvidence != null && GameState.FoundEvidence) nextNode = currentNode.NextIfFoundEvidence;
        currentNode = nextNode;
        ShowNode();
    }

    bool HasChoices()
    {
        return currentNode.Choices != null && currentNode.Choices.Length > 0;
    }

    void EndDialogue(bool makeProgress)
    {
        StopNode();
        StopTypingSound();

        currentNode = null;
        isDialogueActive = false;

        DialogueUIController.SetDialogueText("");
        DialogueUIController.ShowDialogueUI(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (changePlayerState) 
        {
            PlayerStateManager.State = 
                (EndingState.ChosenEnding == Ending.DeathByTea) 
                ? PlayerState.OnlyLookingInput : PlayerState.Normal;
        }
        if (makeProgress && lastNodeEvent != ProgressEvent.None) ProgressManager.CompleteEvent(lastNodeEvent);
        if (isEndingDialogue) TriggerEnd.End();
    }

    void PlayTypingSound()
    {
        if (typingAudioSource == null || typingClips.Length == 0) return;

        typingAudioSource.clip = typingClips[Random.Range(0, typingClips.Length)];
        typingAudioSource.loop = true;
        typingAudioSource.pitch = Random.Range(0.9f, 1.1f);
        typingAudioSource.Play();
    }

    void StopTypingSound()
    {
        if (typingAudioSource == null) return;

        typingAudioSource.Stop();
    }
}
