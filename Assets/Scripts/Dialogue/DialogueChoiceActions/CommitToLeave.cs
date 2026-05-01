using UnityEngine;

public class CommitToLeave : DialogueChoiceAction
{
    [SerializeField] private Task leaveTask;
    [SerializeField] private GameObject stairBlocker;
    [SerializeField] private DoorPivot neighborDoor;

    protected override void Execute()
    {
        // Block going upstairs.
        stairBlocker.SetActive(true);

        // Unlock door.
        neighborDoor.Unlock();

        leaveTask.OnEventStart();
    }
}
