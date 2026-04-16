using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [Header("Milestones")]
    [SerializeField] private ProgressionMilestone[] milestones;

    [Header("System References")]
    [SerializeField] private TasksManager tasksManager;
    [SerializeField] private SpiritController spiritController;

    private readonly ProgressState state = new ProgressState();

    void Awake()
    {
        if (tasksManager == null) tasksManager = FindAnyObjectByType<TasksManager>();
        if (spiritController == null) spiritController = FindAnyObjectByType<SpiritController>();
    }

    void OnEnable()
    {
        ProgressionEvents.OnSignalRaised += HandleSignalRaised;
        TasksEvents.OnTaskStart += HandleTaskStarted;
        TasksEvents.OnTaskProgress += HandleTaskProgressed;
        TasksEvents.OnTaskComplete += HandleTaskCompleted;
    }

    void OnDisable()
    {
        ProgressionEvents.OnSignalRaised -= HandleSignalRaised;
        TasksEvents.OnTaskStart -= HandleTaskStarted;
        TasksEvents.OnTaskProgress -= HandleTaskProgressed;
        TasksEvents.OnTaskComplete -= HandleTaskCompleted;
    }

    void Start()
    {
        EvaluateMilestones();
    }

    void HandleTaskStarted(TaskData task)
    {
        ProgressionEvents.RaiseSignal("task:start:" + task.Id);
    }

    void HandleTaskProgressed(TaskData task)
    {
        ProgressionEvents.RaiseSignal("task:progress:" + task.Id);
    }

    void HandleTaskCompleted(TaskData task)
    {
        ProgressionEvents.RaiseSignal("task:complete:" + task.Id);
    }

    void HandleSignalRaised(string signalId)
    {
        state.AddSignal(signalId);
        EvaluateMilestones();
    }

    void EvaluateMilestones()
    {
        bool completedAnotherMilestone;

        do
        {
            completedAnotherMilestone = false;

            if (milestones == null) return;

            foreach (ProgressionMilestone milestone in milestones)
            {
                if (milestone == null || !milestone.CanComplete(state)) continue;

                CompleteMilestone(milestone);
                completedAnotherMilestone = true;
                break;
            }
        }
        while (completedAnotherMilestone);
    }

    void CompleteMilestone(ProgressionMilestone milestone)
    {
        if (!state.CompleteMilestone(milestone.Id)) return;

        ProgressionEvents.CompleteMilestone(milestone.Id);

        if (milestone.RaiseSignalOnCompletion)
        {
            ProgressionEvents.RaiseSignal("milestone:complete:" + milestone.Id);
        }

        milestone.ExecuteActions(this);
    }

    public void TryCompleteMilestoneById(string milestoneId)
    {
        if (milestones == null || string.IsNullOrEmpty(milestoneId)) return;

        foreach (ProgressionMilestone milestone in milestones)
        {
            if (milestone == null || milestone.Id != milestoneId) continue;
            CompleteMilestone(milestone);
            return;
        }
    }

    public void StartTask(Task task)
    {
        if (task == null || tasksManager == null) return;
        tasksManager.StartExternalTask(task);
    }

    public void TriggerSpiritEvent(SpiritEventType eventType, SpiritController overrideController = null)
    {
        SpiritController controller = overrideController != null ? overrideController : spiritController;
        if (controller == null) return;

        controller.TriggerEvent(eventType);
    }

    public void OpenDoor(DoorPivot door)
    {
        if (door == null) return;
        door.Open();
    }

    public void StartNeighborDialogue(Neighbor neighbor)
    {
        if (neighbor == null) return;
        neighbor.StartDialogue();
    }

    public void SpawnPrefab(GameObject prefab, Transform spawnPoint)
    {
        if (prefab == null) return;

        if (spawnPoint == null)
        {
            Instantiate(prefab);
            return;
        }

        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }

    public bool HasSignal(string id)
    {
        return state.HasSignal(id);
    }

    public bool HasMilestone(string id)
    {
        return state.HasMilestone(id);
    }
}
