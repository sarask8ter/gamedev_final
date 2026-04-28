using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SwitchCameraForDuration", menuName = "Event Actions/Others/Switch To Camera")]
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