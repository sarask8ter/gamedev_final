using UnityEngine;

[CreateAssetMenu(fileName = "TimedTask", menuName = "Tasks/Timed Task")]
public class TimedTask : Task
{
    [SerializeField] private float autoCompleteDelay = 3f;

    protected override void PreStartTask()
    {
        DelayHelper.Delay(autoCompleteDelay, CompleteTask);
    }

    protected override void PreCompleteTask()
    {
        // nothing to unsubscribe from
    }
}