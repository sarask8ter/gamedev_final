using UnityEngine;

public class EventsSetup : MonoBehaviour
{
    [SerializeField] private float startDelay;
    [SerializeField] private EventAction[] eventActions;

    void Start()
    {
        // Set up tasks.
        foreach (var eventAction in eventActions)
        {
            ProgressManager.SubscribeToStart(eventAction.TriggeringEvent, () => eventAction.OnEventStart());
        }

        // Start Game.
        CoroutineHelper.Delay(startDelay, () => ProgressManager.CompleteEvent(ProgressEvent.GameStart));
    }
}