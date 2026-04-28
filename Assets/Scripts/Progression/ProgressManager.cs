using System;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] private float startDelay;
    [SerializeField] private ProgressEvent[] sequence;
    private int currEvtIdx;
    private Action<ProgressEvent> onProgressEventStarted;
    public static Action<ProgressEvent> OnProgressEventStarted { get => _instance.onProgressEventStarted; set => _instance.onProgressEventStarted = value; }
    private Action<ProgressEvent> onProgressEventCompleted;
    public static Action<ProgressEvent> OnProgressEventCompleted { get => _instance.onProgressEventCompleted; set => _instance.onProgressEventCompleted = value; }

    private Dictionary<ProgressEvent, Action> startListeners = new();
    private Dictionary<ProgressEvent, Action> endListeners = new();
    private static ProgressManager _instance;

    public static void SubscribeToStart(ProgressEvent evt, Action callback)
    {
        if (callback == null) return;

        if (_instance.startListeners.TryGetValue(evt, out var existing)) _instance.startListeners[evt] = existing + callback;
        else _instance.startListeners[evt] = callback;
    }

    public static void SubscribeToEnd(ProgressEvent evt, Action callback)
    {
        if (callback == null) return;

        if (_instance.endListeners.TryGetValue(evt, out var existing)) _instance.endListeners[evt] = existing + callback;
        else _instance.endListeners[evt] = callback;
    }

    public static void CompleteEvent(ProgressEvent evt)
    {
        _instance._CompleteEvent(evt);
    }

    void _CompleteEvent(ProgressEvent evt)
    {
        if (currEvtIdx < 0 || currEvtIdx >= sequence.Length || evt == ProgressEvent.None) return;
        if (sequence[currEvtIdx] != evt)
        {
            Debug.LogError("Trying to complete event " + evt + " which doesn't match current event " + sequence[currEvtIdx]);
            return;
        }

        onProgressEventCompleted?.Invoke(evt);
        if (endListeners.TryGetValue(evt, out var callback)) callback?.Invoke();

        startListeners.Remove(evt);
        endListeners.Remove(evt);

        currEvtIdx++;
        InvokeCurrEvent();
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        currEvtIdx = Mathf.Max(0, Array.IndexOf(sequence, ProgressEvent.GameStart));
    }

    void OnEnable()
    {
        onProgressEventStarted += LogEventStart;
        onProgressEventCompleted += LogEventComplete;
    }

    void OnDisable()
    {
        onProgressEventStarted -= LogEventStart;
        onProgressEventCompleted -= LogEventComplete;
    }

    void InvokeCurrEvent()
    {
        if (currEvtIdx < 0 || currEvtIdx >= sequence.Length) return;
        var evt = sequence[currEvtIdx];

        onProgressEventStarted?.Invoke(evt);
        if (startListeners.TryGetValue(evt, out var callback)) callback?.Invoke();
    }

    void LogEventStart(ProgressEvent evt)
    {
        Debug.Log("Event started: " + evt);
    }

    void LogEventComplete(ProgressEvent evt)
    {
        Debug.Log("Event completed: " + evt);
    }
}
