using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksManager : MonoBehaviour
{
    [SerializeField] private float taskDelay;
    [SerializeField] List<Task> tasks;
    private int currTaskIdx = 0;
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
        StartCoroutine(StartNextTask());
    }

    void NextTask(TaskData _)
    {
        currTaskIdx++;
        StartCoroutine(StartNextTask());
    }

    IEnumerator StartNextTask()
    {
        yield return new WaitForSeconds(taskDelay);
        if (currTaskIdx < tasks.Count) tasks[currTaskIdx].StartTask();
    }
}
