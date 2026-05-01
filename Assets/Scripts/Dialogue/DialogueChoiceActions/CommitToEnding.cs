using UnityEngine;

public class CommitToEnding : DialogueChoiceAction
{
    [SerializeField] private Ending ending;
    protected override void Execute()
    {
        EndingState.ChosenEnding = ending;
    }
}
