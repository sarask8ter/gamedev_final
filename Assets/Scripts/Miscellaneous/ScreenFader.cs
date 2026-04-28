using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class ScreenFader : MonoBehaviour
{
    private static ScreenFader _instance;

    private Image fadeImage;
    private Coroutine currentFade;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        fadeImage = GetComponent<Image>();
        fadeImage.raycastTarget = false;
    }

    public static Coroutine FadeOut(float duration)
    {
        return _instance.StartFade(1f, duration);
    }

    public static Coroutine FadeIn(float duration)
    {
        return _instance.StartFade(0f, duration);
    }

    private Coroutine StartFade(float targetAlpha, float duration)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        currentFade = StartCoroutine(Fade(targetAlpha, duration));
        return currentFade;
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);
            float eased = Mathf.SmoothStep(0f, 1f, t);

            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, eased));

            yield return null;
        }

        SetAlpha(targetAlpha);
        currentFade = null;
    }

    private void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}