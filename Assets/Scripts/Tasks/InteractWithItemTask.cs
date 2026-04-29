using UnityEngine;

[CreateAssetMenu(fileName = "InteractWithItemTask", menuName = "Tasks/Interact With Item")]
public class InteractWithItemTask : Task
{
    [SerializeField] private ItemName item;

    protected override void PreStartTask()
    {
        Debug.Log($"Subscribing task {name} instance {GetInstanceID()}");

        TasksEvents.OnItemInteract -= HandleProgress;
        TasksEvents.OnItemInteract += HandleProgress;
    }

    protected override void PreCompleteTask()
    {
        TasksEvents.OnItemInteract -= HandleProgress;
    }

    protected void HandleProgress(ItemName placedItem)
    {
        if (placedItem != item) return;

        Debug.Log("Pizza task progressing");

        TasksEvents.OnItemInteract -= HandleProgress;
        TasksEvents.OnTaskProgress?.Invoke(CompileTaskData());
        CompleteTask();
    }

    // void OnDisable()
    // {
    //     TasksEvents.OnItemInteract -= HandleProgress;
    // }
}
