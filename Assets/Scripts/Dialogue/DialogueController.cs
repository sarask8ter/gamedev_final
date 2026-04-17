using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{

    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;
    private static DialogueController _instance;

    void Awake()
    {
        if (_instance == null)
        {
           _instance = this; 
        }
        else
        {
            Destroy(gameObject); // Make sure only one instance
        }
    }

    public static void ShowDialogueUI(bool show)
    {
        _instance.dialoguePanel.SetActive(show); // Toggle UI visibility
    }

    public static void SetNPCInfo(string npcName)
    {
        _instance.nameText.text = npcName;
    }

    public static void SetDialogueText(string text)
    {
        _instance.dialogueText.text = text;
    }

    public static void ClearChoices()
    {
        foreach (Transform child in _instance.choiceContainer) Destroy(child.gameObject);
    }

    public static void CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(_instance.choiceButtonPrefab, _instance.choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
    }

    public static void StartDialogue(string npcName)
    {
        ShowDialogueUI(true);
        SetNPCInfo(npcName);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerStateManager.State = PlayerState.Dialogue;
    }
}
