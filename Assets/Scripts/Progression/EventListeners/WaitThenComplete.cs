using UnityEngine;

public class WaitThenComplete : EventAction
{
    [SerializeField] private float duration;

    public override void OnEventStart()
    {
        CoroutineHelper.Delay(duration, CompleteEvent);
    }
}