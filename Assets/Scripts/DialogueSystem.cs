using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject choice1Button;
    public GameObject choice2Button;
    public GameObject choice3Button;

    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI choice1Text;
    public TextMeshProUGUI choice2Text;
    public TextMeshProUGUI choice3Text;

    private System.Action<int> onChoiceSelected;

    public IEnumerator StartDialogue(string speaker, string[] lines)
    {
        StopAllCoroutines();
        
        speakerNameText.text = speaker;

        yield return StartCoroutine(TypeLines(lines));
    }

    public IEnumerator TypeLines(string[] lines)
    {
        dialogueText.text = "";

        foreach (string line in lines)
        {
            foreach (char c in line)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(0.03f);
            }

            dialogueText.text += "\n\n";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ShowChoices(string option1, string option2, string option3, System.Action<int> callback)
    {
        choice1Button.SetActive(true);
        choice2Button.SetActive(true);
        choice3Button.SetActive(true);

        choice1Text.text = option1;
        choice2Text.text = option2;
        choice3Text.text = option3;

        onChoiceSelected = callback;
    }

    public void ChooseOption(int choice)
    {
        choice1Button.SetActive(false);
        choice2Button.SetActive(false);
        choice3Button.SetActive(false);

        onChoiceSelected?.Invoke(choice);
    }
}