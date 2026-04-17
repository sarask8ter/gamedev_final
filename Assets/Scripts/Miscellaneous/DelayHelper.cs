using System;
using System.Collections;
using UnityEngine;

public class DelayHelper : MonoBehaviour
{
    private static DelayHelper _instance;

    // Essentially, bootstrap a DelayHelper if it doesn't alreayd exist.
    private static DelayHelper instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("DelayHelper");
                _instance = obj.AddComponent<DelayHelper>();
            }
            return _instance;
        }
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