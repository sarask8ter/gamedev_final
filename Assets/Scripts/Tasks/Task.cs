using UnityEngine;

public abstract class Task : ScriptableObject
{
    [SerializeField] protected string description;
    [SerializeField] protected string progressText;

    [SerializeField] protected string taskId;

    public string TaskId => taskId;
    public string Description => description;
    public string ProgressText => progressText;

    public abstract void StartTask();

    protected TaskData CompileTaskData()
    {
        return new TaskData(taskId, description, progressText);
    }
}
