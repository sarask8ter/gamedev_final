using UnityEngine;

public enum SpiritEventType
{
    Random,
    FlickerLights,
    SlamDoor,
    KnockCabinet,
    ShakeObject,
}

public class SpiritController : MonoBehaviour
{
    [SerializeField] private bool playRandomEvents = false;
    [SerializeField] private float randomEventStartDelay = 5f;
    [SerializeField] private float randomEventRepeatDelay = 10f;

    public LightSwitch[] lights;
    public Door[] doors;
    public Cabinet[] cabinets;

    void Start()
    {
        if (playRandomEvents)
        {
            InvokeRepeating(nameof(DoRandomEvent), randomEventStartDelay, randomEventRepeatDelay);
        }
    }

    // Currently does random events every 10 seconds invoked at the start; we can change based on the storyline how we want the spirit events be triggered later on
    public void DoRandomEvent()
    {
        TriggerEvent((SpiritEventType)Random.Range(1, 5));
    }

    public void TriggerEvent(SpiritEventType eventType)
    {
        Debug.Log("Spirit event: " + eventType);

        switch (eventType)
        {
            case SpiritEventType.Random:
                DoRandomEvent();
                break;
            case SpiritEventType.FlickerLights:
                FlickerLights();
                break;
            case SpiritEventType.SlamDoor:
                SlamDoor();
                break;
            case SpiritEventType.KnockCabinet:
                KnockCabinet();
                break;
            case SpiritEventType.ShakeObject:
                ShakeObject();
                break;
        }
    }

    void FlickerLights()
    {
        foreach (var light in lights)
        {
            light.Flicker(2f);
        }
    }

    void SlamDoor()
    {
        if (doors == null || doors.Length == 0) return;
        doors[Random.Range(0, doors.Length)].Slam();
    }

    void KnockCabinet()
    {
        if (cabinets == null || cabinets.Length == 0) return;
        cabinets[Random.Range(0, cabinets.Length)].KnockOver();
    }

    void ShakeObject()
    {
        if (cabinets == null || cabinets.Length == 0) return;
        // simple shake example
        Transform obj = cabinets[0].transform;

        StartCoroutine(Shake(obj));
    }

    System.Collections.IEnumerator Shake(Transform obj)
    {
        Vector3 original = obj.position;

        for (int i = 0; i < 20; i++)
        {
            obj.position = original + Random.insideUnitSphere * 0.1f;
            yield return new WaitForSeconds(0.02f);
        }

        obj.position = original;
    }
}
