using UnityEngine;

[CreateAssetMenu(fileName = "InteractWithItemTask", menuName = "Event Actions/Tasks/Interact With Item")]
public class InteractWithItemTask : Task
{
    [SerializeField] private ItemName item;

    protected override void PreStartTask()
    {
        TasksEvents.OnItemInteract += HandleProgress;
    }

    protected override void PreCompleteTask()
    {
        TasksEvents.OnItemInteract -= HandleProgress;
    }

    protected void HandleProgress(ItemName interactedItem)
    {
        if (interactedItem != item) return;
        TasksEvents.OnTaskProgress?.Invoke(CompileTaskData());
        CompleteTask();
    }

    void OnDisable()
    {
        TasksEvents.OnItemInteract -= HandleProgress;
    }
}
