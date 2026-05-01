using UnityEngine;

public abstract class EventActionScriptable : ScriptableObject, IEventListener
{
    public ProgressEvent TriggeringEvent;

    public abstract void OnEventStart();

    protected void CompleteEvent()
    {
        ProgressManager.CompleteEvent(TriggeringEvent);
    }
}