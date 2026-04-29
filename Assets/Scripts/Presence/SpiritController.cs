using UnityEngine;
using System.Collections;

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
    public DoorPivot[] doors;
    public Cabinet[] cabinets;

    [Header("Pizza Haunting")]
    [SerializeField] float minObjectDelay = 6f;
    [SerializeField] float maxObjectDelay = 15f;

    bool poltergeistActive;

    [Header("Whispers")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform bathroom;
    [SerializeField] private AudioClip whisperClip;
    [SerializeField] private float maxWhisperDistance = 5f;
    private AudioSource whisperAudio;

    void Start()
    {
        ProgressManager.SubscribeToStart(
            ProgressEvent.PizzaBox,
            StartPizzaHaunting
        );

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
        StartCoroutine(FlickerSequence());
    }


    IEnumerator FlickerSequence()
    {
        foreach (var light in lights)
        {
            light.Flicker(2f);
            yield return new WaitForSeconds(0.25f);
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

    void StartPizzaHaunting()
    {
        whisperAudio = gameObject.AddComponent<AudioSource>();

        whisperAudio.clip = whisperClip;
        whisperAudio.loop = true;
        whisperAudio.spatialBlend = 1f;
        whisperAudio.playOnAwake = false;

        if (poltergeistActive) return;

        poltergeistActive = true;

        Debug.Log("Pizza haunting started");

        StartCoroutine(PoltergeistRoutine());

        whisperAudio.loop = true;
        whisperAudio.Play();

        StartCoroutine(WhisperRoutine());
    }

    IEnumerator PoltergeistRoutine()
    {
        while (poltergeistActive)
        {
            yield return new WaitForSeconds(
                Random.Range(minObjectDelay, maxObjectDelay)
            );

            TriggerRandomDisturbance();
        }
    }

    void TriggerRandomDisturbance()
    {
        int r = Random.Range(0,2);

        if (r == 0)
        {
            KnockCabinet(); 
        }
        if (r == 1)
        {
            ShakeObject();
        }
        else
        {
            FlickerLights();
        }
    }

    IEnumerator WhisperRoutine()
    {
        while (poltergeistActive)
        {
            float dist =
                Vector3.Distance(player.position, bathroom.position);

            float t =
                1f - Mathf.Clamp01(dist / maxWhisperDistance);

            whisperAudio.volume = t;

            // optional creepiness
            whisperAudio.pitch = 0.9f + (t * 0.2f);

            yield return null; // update every frame
        }
    }
}
