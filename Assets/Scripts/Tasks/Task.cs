using UnityEngine;

public abstract class Task : MonoBehaviour
{
    [SerializeField] protected string description;
    [SerializeField] protected string progressText;

    [SerializeField] protected string taskId;

    public abstract void StartTask();

    protected TaskData CompileTaskData()
    {
        return new TaskData(taskId, description, progressText);
    }

    void Start()
    {
        StartTask();
    }
}
