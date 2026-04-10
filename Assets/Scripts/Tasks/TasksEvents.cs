using System;
using UnityEngine;

public class TasksEvents : MonoBehaviour
{
    // Invoked by external objects that will progress tasks.
    public static Action<ItemName> OnItemPlace;

    // Invoked by Task.
    public static Action<Task> OnTaskStart;
    public static Action<Task> OnTaskProgress;
    public static Action<Task> OnTaskComplete;

    // For debugging.
    void Start()
    {
        OnItemPlace += (ItemName item) => { Debug.Log("Item placed: " + item); };
        OnTaskStart += (Task task) => { Debug.Log("Task started: " + task); };
        OnTaskProgress += (Task task) => { Debug.Log("Task progressed: " + task); };
        OnTaskComplete += (Task task) => { Debug.Log("Task completed: " + task); };
    }
}
