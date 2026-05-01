using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoiceManager : MonoBehaviour
{
    private Dictionary<DialogueChoiceId, Action> listeners = new();
    private static DialogueChoiceManager _instance;

    public static void Subscribe(DialogueChoiceId choice, Action callback)
    {
        if (callback == null) return;

        if (_instance.listeners.TryGetValue(choice, out var existing)) _instance.listeners[choice] = existing + callback;
        else _instance.listeners[choice] = callback;
    }

    public static void SelectChoice(DialogueChoiceId choice)
    {
         // Invoke choice then remove subscribers to clean up.
        if (_instance.listeners.TryGetValue(choice, out var callback)) callback?.Invoke();
        _instance.listeners.Remove(choice);
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
}
