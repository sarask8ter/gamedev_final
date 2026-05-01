using System.Collections;
using UnityEngine;

public class WaitThenComplete : EventAction
{
    [SerializeField] private float duration;

    public override void OnEventStart()
    {
        CoroutineHelper.StartCoroutineHelper(DelayThenCompleteEvent());
    }

    IEnumerator DelayThenCompleteEvent()
    {
        yield return new WaitForSeconds(duration);
        CompleteEvent();
    }
}