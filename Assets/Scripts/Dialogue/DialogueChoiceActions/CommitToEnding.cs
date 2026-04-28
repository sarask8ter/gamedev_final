using UnityEngine;

[CreateAssetMenu(fileName = "DialogueChoice - CommitToEnding", menuName = "Dialogue/Choice Actions/Commit to Ending")]
public class CommitToEnding : DialogueChoiceAction
{
    [SerializeField] private Ending ending;
    public override void Execute()
    {
        EndingState.ChosenEnding = ending;
    }
}
