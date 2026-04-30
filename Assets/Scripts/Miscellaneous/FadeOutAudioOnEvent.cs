using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FadeOutAudioOnEvent : MonoBehaviour
{
    [SerializeField] private ProgressEvent fadeEvent;
    [SerializeField] private float fadeDuration;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        ProgressManager.SubscribeToEnd(fadeEvent, () => StartCoroutine(FadeOutCoroutine()));
    }

    private IEnumerator FadeOutCoroutine()
    {
        float time = 0;

        var fadeCurve = AnimationCurve.EaseInOut(0, audioSource.volume, 1, 0);

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            audioSource.volume = fadeCurve.Evaluate(t);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }
}