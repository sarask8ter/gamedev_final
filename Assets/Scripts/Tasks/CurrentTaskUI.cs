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

    void UpdateTaskText(Task task)
    {
        currentTaskText.text = task.Description + " <b>" + task.ProgressText + "</b>";
    }

    void CompleteTask(Task task)
    {
        currentTaskText.text = "<s>" + currentTaskText.text + "</s>";
    }
}
