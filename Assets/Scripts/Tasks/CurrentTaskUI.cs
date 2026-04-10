using TMPro;
using UnityEngine;

public class CurrentTaskUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentTaskText;

    void OnEnable()
    {
        TasksEvents.OnTaskStart += UpdateTaskText;
        TasksEvents.OnTaskProgress += UpdateTaskText;
        TasksEvents.OnTaskComplete += CompleteTask;
    }

    void OnDisable()
    {
        TasksEvents.OnTaskStart -= UpdateTaskText;
        TasksEvents.OnTaskProgress -= UpdateTaskText;
        TasksEvents.OnTaskComplete -= CompleteTask;
    }

    void UpdateTaskText(TaskData task)
    {
        currentTaskText.text = task.Description + " <b>" + task.Progress + "</b>";
    }

    void CompleteTask(TaskData _)
    {
        currentTaskText.text = "<s>" + currentTaskText.text + "</s>";
    }
}
