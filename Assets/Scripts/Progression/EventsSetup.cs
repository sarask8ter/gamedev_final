using UnityEngine;

public class EventsSetup : MonoBehaviour
{
    [SerializeField] private float startDelay;
    [SerializeField] private Task[] tasks;

    void Start()
    {
        // Set up tasks.
        foreach (var task in tasks)
        {
            ProgressManager.SubscribeToStart(task.TriggeringEvent, () => task.StartTask());
        }

        // Start Game.
        DelayHelper.Delay(startDelay, () => ProgressManager.CompleteEvent(ProgressEvent.GameStart));
    }
}