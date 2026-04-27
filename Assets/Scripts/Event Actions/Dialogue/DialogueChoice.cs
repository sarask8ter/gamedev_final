using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string Text;
    public DialogueNode NextNode;
    public DialogueChoiceAction OnChooseChoice;
}

public abstract class DialogueChoiceAction : ScriptableObject
{
    public abstract void Execute();
}