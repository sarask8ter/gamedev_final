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
        cancelAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        // Inspecting state uses Cancel to exit, so don't pause there!
        if ((PlayerStateManager.State != PlayerState.Inspecting) 
            && cancelAction.WasPressedThisFrame()
            && ProgressManager.HasCompleted(ProgressEvent.GameStart))
        {
            PauseOrUnpause();
        }
    }

    public void PauseOrUnpause()
    {
        var doPause = !paused;

        if (doPause) {
            oldState = PlayerStateManager.State;
            PlayerStateManager.State = PlayerState.Pause;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        paused = doPause;
        Time.timeScale = doPause ? 0f : 1f;
        AudioListener.pause = doPause;
        pauseUI.SetActive(doPause);

        if (!doPause) {
            PlayerStateManager.State = oldState;
            if (oldState != PlayerState.Dialogue)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}