using UnityEngine;

[CreateAssetMenu(fileName = "CompleteOnEnding", menuName = "Event Actions/Others/Complete Event If Ending Matches")]
public class CompleteEventBasedOnEnding : EventAction
{
    [SerializeField] private Ending matchEnding;
    public override void OnEventStart()
    {
        if (EndingState.ChosenEnding == matchEnding) CompleteEvent();
    }
}