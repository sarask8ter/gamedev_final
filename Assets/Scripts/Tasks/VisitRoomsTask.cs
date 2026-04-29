using UnityEngine;

[CreateAssetMenu(fileName="VisitRoomsTask", menuName="Tasks/Visit Rooms")]
public class VisitRoomsTask : Task
{
    [SerializeField] private ItemName[] requiredRooms;

    private int count;
    private bool[] visited;

    protected override void PreStartTask()
    {
        count = 0;
        visited = new bool[requiredRooms.Length];

        UpdateProgress();

        TasksEvents.OnItemInteract += HandleRoomVisit;
    }

    protected override void PreCompleteTask()
    {
        TasksEvents.OnItemInteract -= HandleRoomVisit;
    }

    void HandleRoomVisit(ItemName item)
    {
        for(int i=0; i<requiredRooms.Length; i++)
        {
            if(requiredRooms[i] != item)
                continue;

            if(visited[i])
                return;

            visited[i] = true;
            count++;

            UpdateProgress();

            TasksEvents.OnTaskProgress?.Invoke(CompileTaskData());

            if(count >= requiredRooms.Length)
                CompleteTask();

            return;
        }
    }

    void UpdateProgress()
    {
        progressText = "(" + count + "/" + requiredRooms.Length + ")";
    }

    void OnDisable()
    {
        TasksEvents.OnItemInteract -= HandleRoomVisit;
    }
}