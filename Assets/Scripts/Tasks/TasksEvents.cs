using System;
using UnityEngine;

public class TaskData
{
    private string id;
    public string Id => id;

    private string description;
    public string Description => description;

    private string progress;
    public string Progress => progress;

    public TaskData(string id, string description, string progress)
    {
        this.id = id;
        this.description = description;
        this.progress = progress;
    }
}

public delegate void UseTaskData(TaskData task);

public class TasksEvents : MonoBehaviour
{
    // Invoked by external objects that will progress tasks.
    public static Action<ItemName> OnItemPlace;

    // Invoked by Task.
    public static UseTaskData OnTaskStart;
    public static UseTaskData OnTaskProgress;
    public static UseTaskData OnTaskComplete;

    // For debugging.
    void Start()
    {
        OnItemPlace += (item) => { Debug.Log("Item placed: " + item); };
        OnTaskStart += (task) => { Debug.Log("Task started: " + task.Id + " : " + task.Description); };
        OnTaskProgress += (task) => { Debug.Log("Task progressed: " + task.Id + " | " + task.Progress); };
        OnTaskComplete += (task) => { Debug.Log("Task completed: " + task.Id); };
    }
}
