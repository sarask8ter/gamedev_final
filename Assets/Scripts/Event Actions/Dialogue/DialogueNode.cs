using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Scriptable Objects/DialogueNode")]
public class DialogueNode : EventAction
{
    [TextArea]
    public string Text;
    public DialogueNode Next;
    public DialogueChoice[] Choices;
    public bool AutoProgress;
    public float AutoDelay = 1.5f;
    public string SpeakerName;
    public bool AllowClickToNextNode = true;

    public override void OnEventStart()
    {
        DialogueManager.StartDialogue(this);
    }
}