using UnityEngine;

public enum DialogueChoiceId
{
    None,
    TalkToNeighbor,
    IgnoreNeighbor,
    DrinkTeaAndDie,
    LeaveWithoutEvidence,
    LeaveWithEvidence,
    SolveCase,
}

[System.Serializable]
public class DialogueChoice
{
    public string Text;
    public DialogueChoiceId Id;
    public DialogueNode NextNode;
}