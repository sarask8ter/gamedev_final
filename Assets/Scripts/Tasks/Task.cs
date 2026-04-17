using UnityEngine;

public abstract class Task : ScriptableObject
{
    public ProgressEvent TriggeringEvent;

    [SerializeField] protected string description;
    [SerializeField] protected string progressText;

    [SerializeField] protected TaskId id;
    [SerializeField] protected float progressCompletionDelay;

    public TaskId Id => id;
    public string Description => description;
    public string ProgressText => progressText;

    public void StartTask()
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
        DelayHelper.Delay(progressCompletionDelay, () => ProgressManager.CompleteEvent(TriggeringEvent));
    }

    protected TaskData CompileTaskData()
    {
        return new TaskData(id, description, progressText);
    }
}
