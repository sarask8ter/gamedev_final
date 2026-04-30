using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] CanvasGroup introPanel;
    [SerializeField] private float fadeDuration;
    private bool started;

    void Start()
    {
        introPanel.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerStateManager.State = PlayerState.Dialogue; 
    }

    void Update()
    {
        if (started) return;
        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Start Game.
            started = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            StartCoroutine(FadeInGameStart());
        }
    }

    IEnumerator FadeInGameStart()
    {
        PlayerStateManager.State = PlayerState.NoInput;
        yield return FadeCanvas();
        introPanel.gameObject.SetActive(false);
        yield return ScreenFader.FadeIn(fadeDuration);
        PlayerStateManager.State = PlayerState.Normal;
        ProgressManager.CompleteEvent(ProgressEvent.GameStart);
        gameObject.SetActive(false);
    }

    IEnumerator FadeCanvas()
    {
        float startAlpha = introPanel.alpha;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            introPanel.alpha = Mathf.Lerp(startAlpha, 0, t / fadeDuration);
            yield return null;
        }

        introPanel.alpha = 0;
    }
}