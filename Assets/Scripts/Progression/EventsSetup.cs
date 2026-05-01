using UnityEngine;

public class EventsSetup : MonoBehaviour
{
    [SerializeField] private EventActionScriptable[] eventActions;

    void Start()
    {
        // Set up tasks.
        foreach (var eventAction in eventActions)
        {
            ProgressManager.SubscribeToStart(eventAction.TriggeringEvent, eventAction.OnEventStart);
        }
    }
}