using UnityEngine;

public class E2 : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private SpiritController spirit;
    [SerializeField] private LightSwitch[] lights;

    void OnEnable()
    {
        ProgressManager.SubscribeToStart(ProgressEvent.ExploreHouse, StartHauntingPhase);
        ProgressManager.SubscribeToStart(ProgressEvent.MoveInBoxes, StartEarlyHaunting);
    }

    void StartEarlyHaunting()
    {
        spirit.TriggerEvent(SpiritEventType.FlickerLights);
        StartCoroutine(DelayedWhispers());
    }

    void StartHauntingPhase()
    {
        StartCoroutine(HauntingLoop());
    }

    System.Collections.IEnumerator DelayedWhispers()
    {
        yield return new WaitForSeconds(10f);
        spirit.TriggerEvent(SpiritEventType.Random);
    }

    System.Collections.IEnumerator HauntingLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(8f, 15f));

            SpiritEventType eventType =
                (SpiritEventType)Random.Range(1, 5);

            spirit.TriggerEvent(eventType);
        }
    }
}