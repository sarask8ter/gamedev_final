using UnityEngine;

[CreateAssetMenu(fileName = "PlaceItemTask", menuName = "Tasks/Place Item")]
public class PlaceItemTask : Task
{
    [SerializeField] private ItemName item;
    [SerializeField] private int targetCount;

    private int count;

    public override void StartTask()
    {
        count = 0;
        TasksEvents.OnItemPlace -= HandleProgress;
        UpdateProgressText();
        TasksEvents.OnTaskStart?.Invoke(CompileTaskData());
        TasksEvents.OnItemPlace += HandleProgress;
    }

    protected void HandleProgress(ItemName placedItem)
    {
        if (placedItem != item) return;

        count++;
        UpdateProgressText();
        TasksEvents.OnTaskProgress?.Invoke(CompileTaskData());
        if (count >= targetCount) CompleteTask();
    }

    void CompleteTask()
    {
        TasksEvents.OnTaskComplete?.Invoke(CompileTaskData());
        TasksEvents.OnItemPlace -= HandleProgress;
    }

    void UpdateProgressText()
    {
       progressText = "(" + count + "/" + targetCount + ")";  
    }

    void OnDisable()
    {
        TasksEvents.OnItemPlace -= HandleProgress;
    }
}
