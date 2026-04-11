// using UnityEngine;
// using TMPro;

// public class DialogueController : MonoBehaviour
// {
//     public static DialogueController Instance { get; private set; }

//     public GameObject dialoguePanel;
//     public TMP_Text dialogueText, nameText;

//     void Awake()
//     {
//         if (Instance == null) Instance == this;
//         else Destroy(gameObject); // Make sure only one instance
        
//     }

//     public void ShowDialogueUI(bool show)
//     {
//         dialoguePanel.SetActive(show); // Toggle UI visibility
//     }

//     public void SetNPCInfo(string npcName)
//     {
//         nameText.text = npcName;
//     }

//     public void SetDialogueText(string text)
//     {
//         dialogueText.text = text;
//     }
// }
