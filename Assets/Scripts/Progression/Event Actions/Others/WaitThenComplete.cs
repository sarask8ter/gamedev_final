using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitThenComplete", menuName = "Event Actions/Others/Wait Then Complete")]
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