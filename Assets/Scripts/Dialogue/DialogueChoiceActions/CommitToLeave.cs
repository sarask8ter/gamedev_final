using UnityEngine;

[CreateAssetMenu(fileName = "DialogueChoice - Commit to Leave", menuName = "Dialogue/Choice Actions/Commit to Leave")]
public class CommitToLeave : DialogueChoiceAction
{
    public override void Execute()
    {
        // Block going upstairs.
        GameState.StairBlocker.SetActive(true);

        // Unlock door.
        GameState.NeighborDoor.Unlock();
    }
}
