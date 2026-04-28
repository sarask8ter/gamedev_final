using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerEnd : MonoBehaviour
{
    [SerializeField] private string endSceneName;
    [SerializeField] private float fadeDuration;
    private static TriggerEnd _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static void End()
    {
        _instance.StartCoroutine(_instance.EndSequence());
    }

    IEnumerator EndSequence()
    {
        PlayerStateManager.State = PlayerState.NoInput;
        yield return ScreenFader.FadeOut(fadeDuration);
        SceneManager.LoadScene(endSceneName);
    }
}