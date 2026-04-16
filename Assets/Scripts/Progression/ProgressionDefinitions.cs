using System;
using UnityEngine;

public enum ProgressionActionType
{
    RaiseSignal,
    CompleteMilestone,
    StartTask,
    SetGameObjectActive,
    SpawnPrefab,
    OpenDoor,
    TriggerSpiritEvent,
    StartNeighborDialogue,
}

[Serializable]
public class ProgressionAction
{
    [SerializeField] private ProgressionActionType actionType;
    [SerializeField] private string id;
    [SerializeField] private Task task;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private bool activeState = true;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Door door;
    [SerializeField] private SpiritController spiritController;
    [SerializeField] private SpiritEventType spiritEventType = SpiritEventType.Random;
    [SerializeField] private Neighbor neighbor;

    public void Execute(ProgressionManager manager)
    {
        switch (actionType)
        {
            case ProgressionActionType.RaiseSignal:
                ProgressionEvents.RaiseSignal(id);
                break;

            case ProgressionActionType.CompleteMilestone:
                manager.TryCompleteMilestoneById(id);
                break;

            case ProgressionActionType.StartTask:
                manager.StartTask(task);
                break;

            case ProgressionActionType.SetGameObjectActive:
                if (targetObject != null) targetObject.SetActive(activeState);
                break;

            case ProgressionActionType.SpawnPrefab:
                manager.SpawnPrefab(prefab, spawnPoint);
                break;

            case ProgressionActionType.OpenDoor:
                manager.OpenDoor(door);
                break;

            case ProgressionActionType.TriggerSpiritEvent:
                manager.TriggerSpiritEvent(spiritEventType, spiritController);
                break;

            case ProgressionActionType.StartNeighborDialogue:
                manager.StartNeighborDialogue(neighbor);
                break;
        }
    }
}

[Serializable]
public class ProgressionMilestone
{
    [SerializeField] private string id;
    [SerializeField] private string[] requiredSignals;
    [SerializeField] private string[] requiredMilestones;
    [SerializeField] private bool raiseSignalOnCompletion = true;
    [SerializeField] private ProgressionAction[] actions;

    public string Id => id;
    public bool RaiseSignalOnCompletion => raiseSignalOnCompletion;

    public bool CanComplete(ProgressState state)
    {
        if (state.HasMilestone(id)) return false;

        if (requiredSignals != null)
        {
            foreach (string requiredSignal in requiredSignals)
            {
                if (string.IsNullOrEmpty(requiredSignal)) continue;
                if (!state.HasSignal(requiredSignal)) return false;
            }
        }

        if (requiredMilestones != null)
        {
            foreach (string requiredMilestone in requiredMilestones)
            {
                if (string.IsNullOrEmpty(requiredMilestone)) continue;
                if (!state.HasMilestone(requiredMilestone)) return false;
            }
        }

        return true;
    }

    public void ExecuteActions(ProgressionManager manager)
    {
        if (actions == null) return;

        foreach (ProgressionAction action in actions)
        {
            if (action == null) continue;
            action.Execute(manager);
        }
    }
}
