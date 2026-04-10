using System.Collections;
using TMPro;
using UnityEngine;

public class CurrentTaskUI : MonoBehaviour
{
    [SerializeField] private float strikethroughTime;
    private TextMeshProUGUI currentTaskText;

    void Awake()
    {
        currentTaskText = GetComponent<TextMeshProUGUI>();
    }

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
        SetTaskText(task.Description + " ", task.Progress);
    }

    void CompleteTask(TaskData task)
    {
        StartCoroutine(AnimateStrikethrough(task.Description + " ", task.Progress));
    }

    void SetTaskText(string beginning, string bolded)
    {
        currentTaskText.text = beginning + "<b>" + bolded + "</b>";
    }

    IEnumerator AnimateStrikethrough(string description, string progress)
    {
        int endTagIdx = 1;
        const string beginningTag = "<s>";
        const string endTag = "</s>";
        string newDescription = description;

        float delay = strikethroughTime / (description.Length + progress.Length);

        while (endTagIdx <= description.Length)
        {
            newDescription = beginningTag + description.Insert(endTagIdx, endTag);
            SetTaskText(newDescription, progress);
            yield return new WaitForSeconds(delay);

            endTagIdx++;
        }

        endTagIdx = 1;

        while (endTagIdx <= progress.Length)
        {
            string newProgress = beginningTag + progress.Insert(endTagIdx, endTag);
            SetTaskText(newDescription, newProgress);
            yield return new WaitForSeconds(delay);

            endTagIdx++;
        }
    }
}
