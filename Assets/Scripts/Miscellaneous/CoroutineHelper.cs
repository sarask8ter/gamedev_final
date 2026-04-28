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

    public static Coroutine Delay(float seconds, Action action)
    {
        return _instance.StartCoroutine(DelayAction(seconds, action));
    }

    public static void Cancel(Coroutine coroutine)
    {
        if (_instance == null || coroutine == null) return;
        _instance.StopCoroutine(coroutine);
    }

    static IEnumerator DelayAction(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}