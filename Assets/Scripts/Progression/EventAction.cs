using UnityEngine;

public abstract class EventAction : MonoBehaviour, IEventListener
{
    public ProgressEvent TriggeringEvent;

    void Start()
    {
        ProgressManager.SubscribeToStart(TriggeringEvent, OnEventStart);
    }

    public abstract void OnEventStart();
    
    protected void CompleteEvent()
    {
        ProgressManager.CompleteEvent(TriggeringEvent);
    }
}