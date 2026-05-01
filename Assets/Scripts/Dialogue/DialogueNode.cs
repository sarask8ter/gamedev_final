using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/Node")]
public class DialogueNode : EventActionScriptable
{
    [TextArea]
    public string Text;
    public DialogueNode Next;
    public DialogueNode NextIfFoundEvidence;
    public DialogueChoice[] Choices;
    public bool AutoProgress;
    public float AutoDelay = 1.5f;
    public Speaker speaker;
    public bool AllowClickToNextNode = true;
    public bool IsEndingDialogue;
    public bool ChangePlayerState = true;

    public override void OnEventStart()
    {
        DialogueManager.StartDialogue(this, true, ChangePlayerState);
    }
}