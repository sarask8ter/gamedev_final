using UnityEngine;

public abstract class Task : EventAction
{
    [SerializeField] protected string description;
    [SerializeField] protected string progressText;

    [SerializeField] protected float progressCompletionDelay;

    public string Description => description;
    public string ProgressText => progressText;

    public override void OnEventStart()
    {
        PreStartTask();
        TasksEvents.OnTaskStart?.Invoke(CompileTaskData());
    }

    protected abstract void PreStartTask();

    protected abstract void PreCompleteTask();

    protected void CompleteTask()
    {
        PreCompleteTask();
        TasksEvents.OnTaskComplete?.Invoke(CompileTaskData());
        CoroutineHelper.Delay(progressCompletionDelay, () => CompleteEvent());
    }

    protected TaskData CompileTaskData()
    {
        return new TaskData(TriggeringEvent, description, progressText);
    }
}
