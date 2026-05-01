using System.Collections;
using UnityEngine;

public class MoveObject : EventAction
{
    [SerializeField] private GameObject obj;
    [SerializeField] private Transform moveToPoint;
    [SerializeField] private bool completeAfterMove;
    [SerializeField] private float completeDelay;

    public override void OnEventStart()
    {
        MovementHelper.MoveToPoint(obj, moveToPoint, false);
        if (completeAfterMove) CoroutineHelper.StartCoroutineHelper(DelayThenCompleteEvent());
    }

    IEnumerator DelayThenCompleteEvent()
    {
        yield return new WaitForSeconds(completeDelay);
        CompleteEvent();
    }
}