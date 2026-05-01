using UnityEngine;

public class CompleteEventBasedOnEnding : EventAction
{
    [SerializeField] private Ending matchEnding;
    public override void OnEventStart()
    {
        if (EndingState.ChosenEnding == matchEnding) CompleteEvent();
    }
}