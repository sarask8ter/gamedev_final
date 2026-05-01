using System.Collections;
using UnityEngine;

public class SwitchCameraForDuration : EventAction
{
    [SerializeField] private float duration;
    [SerializeField] private GameCamera cam;

    public override void OnEventStart()
    {
        CinemachineCameraSwitcher.SwitchToNewCam(cam);
        CoroutineHelper.StartCoroutineHelper(DelayThenCompleteEvent());
    }

    IEnumerator DelayThenCompleteEvent()
    {
        yield return new WaitForSeconds(duration);
        CompleteEvent();
    }
}