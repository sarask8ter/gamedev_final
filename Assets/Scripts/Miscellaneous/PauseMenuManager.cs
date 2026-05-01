using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private AudioSource backgroundMusic;
    private InputAction cancelAction;
    private bool paused;
    private PlayerState oldState;

    void Awake()
    {
        if (backgroundMusic != null) backgroundMusic.ignoreListenerPause = true;
        cancelAction = InputSystem.actions.FindAction("Cancel");
    }

    void Update()
    {
        // Inspecting state uses Cancel to exit, so don't pause there!
        if ((PlayerStateManager.State != PlayerState.Inspecting) 
            && cancelAction.WasPressedThisFrame()
            && ProgressManager.HasCompleted(ProgressEvent.GameStart))
        {
            PauseOrUnpause(!paused);
        }
    }

    void PauseOrUnpause(bool doPause)
    {
        if (doPause) {
            oldState = PlayerStateManager.State;
            PlayerStateManager.State = PlayerState.NoInput;
        }

        paused = doPause;
        Time.timeScale = doPause ? 0f : 1f;
        AudioListener.pause = doPause;
        pauseUI.SetActive(doPause);

        if (!doPause) PlayerStateManager.State = oldState;
    }
}