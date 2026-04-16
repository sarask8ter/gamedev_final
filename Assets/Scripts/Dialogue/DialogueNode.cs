using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Scriptable Objects/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    [TextArea]
    public string text;

    public DialogueNode next;

    public DialogueChoice[] choices;

    public bool autoProgress;
    public float autoDelay = 1.5f;
    public string speakerName;
}