using UnityEngine;

[CreateAssetMenu(fileName = "TimedTask", menuName = "Event Actions/Tasks/Timed Task")]
public class TimedTask : Task
{
    [SerializeField] private float autoCompleteDelay = 3f;

    protected override void PreStartTask()
    {
        CoroutineHelper.Delay(autoCompleteDelay, CompleteTask);
    }

    protected override void PreCompleteTask() {}
}