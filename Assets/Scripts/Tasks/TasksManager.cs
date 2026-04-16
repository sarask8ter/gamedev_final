using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksManager : MonoBehaviour
{
    [SerializeField] private float taskDelay;
    [SerializeField] private bool autoRunTaskList = true;
    [SerializeField] List<Task> tasks;
    private int currTaskIdx = 0;
    private Task currentTask;
    private static TasksManager _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void OnEnable()
    {
        TasksEvents.OnTaskComplete += NextTask;
    }

    void OnDisable()
    {
        TasksEvents.OnTaskComplete -= NextTask;
    }

    void Start()
    {
        if (autoRunTaskList) StartCoroutine(StartNextTask());
    }

    void NextTask(TaskData taskData)
    {
        if (currentTask == null || taskData.Id != currentTask.TaskId) return;

        if (!autoRunTaskList) return;

        currTaskIdx++;
        StartCoroutine(StartNextTask());
    }

    IEnumerator StartNextTask()
    {
        yield return new WaitForSeconds(taskDelay);
        if (currTaskIdx < tasks.Count)
        {
            currentTask = tasks[currTaskIdx];
            currentTask.StartTask();
        }
    }

    public void StartExternalTask(Task task)
    {
        if (task == null) return;

        StopAllCoroutines();
        currentTask = task;
        currentTask.StartTask();
    }
}
