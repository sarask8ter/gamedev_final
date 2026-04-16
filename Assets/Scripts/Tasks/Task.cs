using UnityEngine;

public abstract class Task : ScriptableObject
{
    [SerializeField] protected string description;
    [SerializeField] protected string progressText;

    [SerializeField] protected string taskId;

    public string TaskId => taskId;
    public string Description => description;
    public string ProgressText => progressText;

    public void StartTask()
    {
        PreStartTask();
        TasksEvents.OnTaskStart?.Invoke(CompileTaskData());
    }

    protected abstract void PreStartTask();

    protected TaskData CompileTaskData()
    {
        return new TaskData(taskId, description, progressText);
    }
}
