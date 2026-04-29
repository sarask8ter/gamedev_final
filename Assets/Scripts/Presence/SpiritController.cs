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
    private HauntState currentState = HauntState.None;

    enum HauntState
    {
        None,
        Pizza,
        Bedroom,
        Bathroom,
        PostBathroom
    }

    public static SpiritController Instance;

    public LightSwitch[] lights;
    public DoorPivot[] doors;
    public Cabinet[] cabinets;

    [Header("Pizza Haunting")]
    [SerializeField] float minObjectDelay = 4f;
    [SerializeField] float maxObjectDelay = 10f;

    bool poltergeistActive;

    [Header("Whisper")]
    [SerializeField] private AudioSource whisperAudio;
    [SerializeField] private AudioClip whisperClip;
    [SerializeField] private Transform player;
    [SerializeField] private Transform bathroom;
    [SerializeField] private float maxWhisperDistance = 7f;

    [Header("Breathing")]
    [SerializeField] private AudioSource breathingAudio;
    [SerializeField] private AudioClip breathingClip;

    [Header("Kitchen Only")]
    [SerializeField] private Cabinet[] kitchenCabinets;
    [SerializeField] private LightSwitch[] kitchenLights;

    void Start()
    {
        ProgressManager.SubscribeToStart(ProgressEvent.PizzaBox, () =>
        {
            SetState(HauntState.Pizza);
            StartPizzaHaunting();
        });

        ProgressManager.SubscribeToStart(ProgressEvent.EnterBedroom, () =>
        {
            SetState(HauntState.Bedroom);
        });

        ProgressManager.SubscribeToStart(ProgressEvent.EnterBathroom, () =>
        {
            SetState(HauntState.Bathroom);
        });
    }

    void Awake()
    {
        Instance = this;
    }

    public void DoRandomEvent()
    {
        SpiritEventType randomEvent =
        (SpiritEventType)Random.Range(1, System.Enum.GetValues(typeof(SpiritEventType)).Length);

        TriggerEvent(randomEvent);
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
        LightSwitch[] targetLights =
            currentState == HauntState.Pizza ? kitchenLights : lights;

        foreach (var light in targetLights)
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
        Cabinet[] target =
            currentState == HauntState.Pizza ? kitchenCabinets : cabinets;

        if (target == null || target.Length == 0) return;

        target[Random.Range(0, target.Length)].KnockOver();
    }

    void ShakeObject()
    {
        if (cabinets == null || cabinets.Length == 0) return;
        Transform obj = cabinets[Random.Range(0, cabinets.Length)].transform;

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
        if (poltergeistActive) return;
        poltergeistActive = true;

        if (whisperAudio == null)
            whisperAudio = gameObject.AddComponent<AudioSource>();

        whisperAudio.clip = whisperClip;
        whisperAudio.loop = true;
        whisperAudio.spatialBlend = 1f;
        whisperAudio.playOnAwake = false;

        Debug.Log("Pizza haunting started");

        StartCoroutine(PoltergeistRoutine());

        whisperAudio.Play();
        StartCoroutine(WhisperRoutine());
    }

    void StartBathroomHaunting()
    {
        minObjectDelay = 2f;
        maxObjectDelay = 6f;
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
        int r = Random.Range(0, 3);

        if (r == 0)
        {
            KnockCabinet(); // kitchen only override later
        }
        else if (r == 1)
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
            float dist = Vector3.Distance(player.position, bathroom.position);

            float t = 1f - Mathf.Clamp01(dist / maxWhisperDistance);

            float targetVolume = Mathf.Lerp(0.05f, 1f, t);
            whisperAudio.volume = Mathf.Lerp(whisperAudio.volume, targetVolume, Time.deltaTime * 3f);

            whisperAudio.pitch = 0.9f + (t * 0.3f);

            yield return null;
        }
    }

    void SetState(HauntState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case HauntState.Pizza:
                minObjectDelay = 6f;
                maxObjectDelay = 15f;
                break;

            case HauntState.Bedroom:
                minObjectDelay = 4f;
                maxObjectDelay = 10f;
                break;

            case HauntState.Bathroom:
                minObjectDelay = 1.5f;
                maxObjectDelay = 5f;
                break;
        }
    }

    public void EndBathroomSequence()
    {
        SetState(HauntState.PostBathroom);

        poltergeistActive = false;

        StartCoroutine(BreathingRoutine());
    }

    IEnumerator BreathingRoutine()
    {
        if (breathingClip == null)
        {
            Debug.LogWarning("No breathing clip assigned!");
            yield break;
        }

        if (breathingAudio == null)
        {
            breathingAudio = gameObject.AddComponent<AudioSource>();
        }

        breathingAudio.clip = breathingClip;
        breathingAudio.loop = true;
        breathingAudio.spatialBlend = 0.3f;
        breathingAudio.rolloffMode = AudioRolloffMode.Linear;
        breathingAudio.volume = 1f;
        breathingAudio.playOnAwake = false;

        breathingAudio.Play();

        float t = 0f;

        while (currentState == HauntState.PostBathroom)
        {
            t += Time.deltaTime;

            float intensity = Mathf.Lerp(0.5f, 0.05f, t / 20f);
            breathingAudio.volume = intensity;
        }

        breathingAudio.Stop();
    }
}
