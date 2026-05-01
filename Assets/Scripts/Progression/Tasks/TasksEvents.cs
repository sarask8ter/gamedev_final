using System;
using UnityEngine;

public class TaskData
{
    private ProgressEvent triggeringEvent;
    public ProgressEvent TriggeringEvent => triggeringEvent;

    private string description;
    public string Description => description;

    private string progress;
    public string Progress => progress;

    public TaskData(ProgressEvent triggeringEvent, string description, string progress)
    {
        this.triggeringEvent = triggeringEvent;
        this.description = description;
        this.progress = progress;
    }
}

public delegate void UseTaskData(TaskData task);

public class TasksEvents : MonoBehaviour
{
    // Invoked by external objects that will progress tasks.
    public static Action<ItemName> OnItemPlace;
    public static Action<ItemName> OnItemInteract;

    // Invoked by Task.
    public static UseTaskData OnTaskStart;
    public static UseTaskData OnTaskProgress;
    public static UseTaskData OnTaskComplete;

    void OnEnable()
    {
        OnItemPlace += LogItem;
        OnTaskStart += LogTaskStart;
        OnTaskProgress += LogTaskProgress;
        OnTaskComplete += LogTaskComplete;
    }

    void OnDisable()
    {
        OnItemPlace -= LogItem;
        OnTaskStart -= LogTaskStart;
        OnTaskProgress -= LogTaskProgress;
        OnTaskComplete -= LogTaskComplete;
    }

    void LogItem(ItemName item)
    {
        Debug.Log("Item updated: " + item);
    }

    void LogTaskStart(TaskData task)
    {
        Debug.Log($"Task started: {task.TriggeringEvent} : {task.Description}");
    }

    void LogTaskProgress(TaskData task)
    {
        Debug.Log($"Task progressed: {task.TriggeringEvent} | {task.Progress}");
    }

    void LogTaskComplete(TaskData task)
    {
        Debug.Log($"Task completed: {task.TriggeringEvent}");
    }
}
