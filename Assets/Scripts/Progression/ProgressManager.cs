using System;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] private float startDelay;
    [SerializeField] private ProgressEvent[] sequence;
    [SerializeField] private int currEvtIdx; // Editable from editor to allow "jumping ahead"
    public event Action<ProgressEvent> OnProgressEventStarted;
    public event Action<ProgressEvent> OnProgressEventCompleted;

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

        OnProgressEventCompleted?.Invoke(evt);
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

    void OnEnable()
    {
        OnProgressEventStarted += LogEventStart;
        OnProgressEventCompleted += LogEventComplete;
    }

    void OnDisable()
    {
        OnProgressEventStarted -= LogEventStart;
        OnProgressEventCompleted -= LogEventComplete;
    }

    void InvokeCurrEvent()
    {
        if (currEvtIdx < 0 || currEvtIdx >= sequence.Length) return;
        var evt = sequence[currEvtIdx];

        OnProgressEventStarted?.Invoke(evt);
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
