using System;
using System.Collections;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper _instance;

    // Essentially, bootstrap a DelayHelper if it doesn't alreayd exist.
    private static CoroutineHelper instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("DelayHelper");
                _instance = obj.AddComponent<CoroutineHelper>();
            }
            return _instance;
        }
    }

    public static void StartCoroutineHelper(IEnumerator enumerator)
    {
        instance.StartCoroutine(enumerator);
    }

    public static void Delay(float seconds, Action action)
    {
        instance.StartCoroutine(DelayAction(seconds, action));
    }

    static IEnumerator DelayAction(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}