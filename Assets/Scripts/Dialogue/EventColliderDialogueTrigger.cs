using UnityEngine;

public class EventColliderDialogueTrigger : DialogueColliderTrigger
{
    [SerializeField] private ProgressEvent evt;
    protected override void PostTrigger()
    {
        ProgressManager.CompleteEvent(evt);
    }
}