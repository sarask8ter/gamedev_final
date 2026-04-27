using System;
using System.Collections;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper _instance;

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

    public static void StartCoroutineHelper(IEnumerator enumerator)
    {
        _instance.StartCoroutine(enumerator);
    }

    public static void Delay(float seconds, Action action)
    {
        _instance.StartCoroutine(DelayAction(seconds, action));
    }

    static IEnumerator DelayAction(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}