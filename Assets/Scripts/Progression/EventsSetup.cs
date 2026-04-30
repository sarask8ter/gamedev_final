using UnityEngine;

public class EventsSetup : MonoBehaviour
{
    [SerializeField] private EventAction[] eventActions;

    void Start()
    {
        // Set up tasks.
        foreach (var eventAction in eventActions)
        {
            ProgressManager.SubscribeToStart(eventAction.TriggeringEvent, eventAction.OnEventStart);
        }
    }
}