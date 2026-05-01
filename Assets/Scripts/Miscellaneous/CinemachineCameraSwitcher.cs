using UnityEngine;

public enum GameCamera
{
    PlayerCam,
    BodyOnlyCam,
    BodyAndNeighborCam,
}

public class CinemachineCameraSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] allCams;

    private static CinemachineCameraSwitcher _instance;

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

    public static void SwitchToNewCam(GameCamera cam)
    {
        _instance._SwitchToNewCam((int)cam);
    }

    void _SwitchToNewCam(int idx)
    {
        Debug.Log("Switching to " + idx + "-th cam");
        PlayerStateManager.State = PlayerState.NoInput;
        for (int i = 0; i < allCams.Length; i++) 
        {
            var cam = allCams[i];
            if (cam != null) cam.SetActive(i == idx);
        }
    }
}