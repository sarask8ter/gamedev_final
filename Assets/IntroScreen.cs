using UnityEngine;
using UnityEngine.InputSystem;

public class IntroScreen : MonoBehaviour
{
    [SerializeField] GameObject introPanel;

    bool waitingForInput = true;

    void Start()
    {
        introPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerStateManager.State = PlayerState.Dialogue; 
    }

    void Update()
    {
        if (!waitingForInput) return;

        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            BeginGame();
        }
    }

    void BeginGame()
    {
        waitingForInput = false;

        introPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerStateManager.State = PlayerState.Normal;

        ProgressManager.CompleteEvent(ProgressEvent.GameStart);
    }
}