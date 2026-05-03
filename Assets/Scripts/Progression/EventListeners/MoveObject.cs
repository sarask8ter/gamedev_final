using UnityEngine;

public class MoveObject : EventAction
{
    [SerializeField] private GameObject obj;
    [SerializeField] private Transform moveToPoint;
    [SerializeField] private bool completeAfterMove;
    [SerializeField] private float completeDelay;
    [SerializeField] private GameObject moveSFX;

    public override void OnEventStart()
    {
        MovementHelper.MoveToPoint(obj, moveToPoint, false);
        if (moveSFX != null) Instantiate(moveSFX);
        if (completeAfterMove) CoroutineHelper.Delay(completeDelay, CompleteEvent);
    }
}