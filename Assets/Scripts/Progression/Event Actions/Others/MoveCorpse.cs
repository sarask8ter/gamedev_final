using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveCorpse", menuName = "Event Actions/Others/Move Corpse")]
public class MoveObject : EventAction
{
    [SerializeField] private float duration;

    public override void OnEventStart()
    {
        MovementHelper.MoveToPoint(GameState.Corpse, GameState.CorpseUprightPoint, false);
        CoroutineHelper.StartCoroutineHelper(DelayThenCompleteEvent());
    }

    IEnumerator DelayThenCompleteEvent()
    {
        yield return new WaitForSeconds(duration);
        CompleteEvent();
    }
}