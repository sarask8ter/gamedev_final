using UnityEngine;

public class PlaceItemTask : Task
{
    [SerializeField] private ItemName item;
    [SerializeField] private int targetCount;

    private int count;

    public override void StartTask()
    {
        count = 0;
        UpdateProgressText();
        TasksEvents.OnTaskStart?.Invoke(this);
        TasksEvents.OnItemPlace += HandleProgress;
    }

    protected void HandleProgress(ItemName placedItem)
    {
        if (placedItem != item) return;

        count++;
        UpdateProgressText();
        TasksEvents.OnTaskProgress?.Invoke(this);
        if (count >= targetCount) CompleteTask();
    }

    void CompleteTask()
    {
        TasksEvents.OnTaskComplete?.Invoke(this);
        TasksEvents.OnItemPlace -= HandleProgress;
    }

    void UpdateProgressText()
    {
       ProgressText = "(" + count + "/" + targetCount + ")";  
    }
}
