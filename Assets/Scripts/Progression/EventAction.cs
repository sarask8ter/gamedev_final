using UnityEngine;

public abstract class EventAction : ScriptableObject
{
    public ProgressEvent TriggeringEvent;

    public abstract void OnEventStart();

    protected void CompleteEvent()
    {
        ProgressManager.CompleteEvent(TriggeringEvent);
    }
}