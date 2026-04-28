using System.Collections;
using UnityEngine;

public class EventsSetup : MonoBehaviour
{
    [SerializeField] private float startDelay;
    [SerializeField] private float fadeDuration;
    [SerializeField] private EventAction[] eventActions;

    void Start()
    {
        // Set up tasks.
        foreach (var eventAction in eventActions)
        {
            ProgressManager.SubscribeToStart(eventAction.TriggeringEvent, () => eventAction.OnEventStart());
        }

        // Start Game.
        CoroutineHelper.Delay(startDelay, () => StartCoroutine(FadeInGameStart()));
    }

    IEnumerator FadeInGameStart()
    {
        PlayerStateManager.State = PlayerState.NoInput;
        yield return ScreenFader.FadeIn(fadeDuration);
        PlayerStateManager.State = PlayerState.Normal;
        ProgressManager.CompleteEvent(ProgressEvent.GameStart);
    }
}