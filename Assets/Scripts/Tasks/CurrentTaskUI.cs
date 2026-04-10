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
        const string beginningTag = "<s>";
        const string endTag = "</s>";
        string newDescription = description;
        string newProgress = progress;

        float delay = strikethroughTime / (description.Length + progress.Length);
        bool isDesc = true;

        foreach (var str in new string[]{description, progress})
        {
            int endTagIdx = 1;

            while (endTagIdx <= str.Length)
            {
                string strWithTags = beginningTag + str.Insert(endTagIdx, endTag);

                if (isDesc) newDescription = strWithTags;
                else newProgress = strWithTags;

                SetTaskText(newDescription, newProgress);
                yield return new WaitForSeconds(delay);

                endTagIdx++;
            }

            isDesc = false;
        }
    }
}
