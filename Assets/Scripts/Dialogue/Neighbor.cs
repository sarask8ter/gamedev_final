using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Neighbor : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName;
    [SerializeField] private DialogueNode startNode;

    private DialogueController dialogueUI;
    private DialogueNode currentNode;

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

    void Start()
    {
        dialogueUI = DialogueController.Instance;
    }

    void Update()
    {
        if (isDialogueActive && nextLineAction.WasPressedThisFrame())
        {
            if (!HasChoices())
                GoToNextNode();
        }
    }

    public void Interact(PlayerInteractor player)
    {
        if (!canStartDialogue || isDialogueActive) return;

        StartDialogue();
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        currentNode = startNode;

        interactionPrompt.HideEUI();

        dialogueUI.StartDialogue(npcName);
        ShowNode();
    }

    void ShowNode()
    {
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        StopAllCoroutines();
        dialogueUI.ClearChoices();

        StartCoroutine(TypeLine(currentNode.text));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        for (int i = 0; i < line.Length; i++)
        {
            dialogueUI.SetDialogueText(line.Substring(0, i + 1));
            yield return new WaitForSeconds(0.02f);
        }

        isTyping = false;

        if (HasChoices())
        {
            ShowChoices();
        }
        else if (currentNode.autoProgress)
        {
            Invoke(nameof(GoToNextNode), currentNode.autoDelay);
        }
    }

    void ShowChoices()
    {
        foreach (var choice in currentNode.choices)
        {
            dialogueUI.CreateChoiceButton(choice.text, () =>
            {
                currentNode = choice.nextNode;
                ShowNode();
            });
        }
    }

    void GoToNextNode()
    {
        currentNode = currentNode.next;
        ShowNode();
    }

    bool HasChoices()
    {
        return currentNode.choices != null && currentNode.choices.Length > 0;
    }

    void EndDialogue()
    {
        StopAllCoroutines();
        CancelInvoke();

        isDialogueActive = false;

        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerStateManager.State = PlayerState.Normal;

        canStartDialogue = false;
        StartCoroutine(ResetDialogueCooldown());
    }

    IEnumerator ResetDialogueCooldown()
    {
        yield return new WaitForSeconds(1f);
        canStartDialogue = true;
    }
}