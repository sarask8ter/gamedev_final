using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] GameObject introPanel;
    [SerializeField] private float startDelay;
    [SerializeField] private float fadeDuration;
    private bool started;

    void Start()
    {
        introPanel.SetActive(true);

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
    
            introPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CoroutineHelper.Delay(startDelay, () => StartCoroutine(FadeInGameStart()));
        }
    }

    IEnumerator FadeInGameStart()
    {
        PlayerStateManager.State = PlayerState.NoInput;
        yield return ScreenFader.FadeIn(fadeDuration);
        PlayerStateManager.State = PlayerState.Normal;
        ProgressManager.CompleteEvent(ProgressEvent.GameStart);
        gameObject.SetActive(false);
    }
}