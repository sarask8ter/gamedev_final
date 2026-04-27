using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    private Image fadeImage;
    private static ScreenFader _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            fadeImage = GetComponent<Image>();
        }
    }

    public static IEnumerator FadeOutRoutine(float duration)
    {
        yield return _instance.Fade(1f, duration);
    }

    public static IEnumerator FadeInRoutine(float duration)
    {
        yield return _instance.Fade(0f, duration);
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

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, eased);
            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}