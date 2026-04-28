public class BloodiedDoorDialogueTrigger : DialogueColliderTrigger
{
    protected override void PostTrigger()
    {
        ProgressManager.CompleteEvent(ProgressEvent.StayTheNightDecision);
    }
}