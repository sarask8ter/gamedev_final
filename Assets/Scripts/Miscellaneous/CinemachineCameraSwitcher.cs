using UnityEngine;

public class CinemachineCameraSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] allCams;
    [SerializeField] private bool hidePlayer;

    public void SwitchToNewCam(int idx)
    {
        PlayerStateManager.State = PlayerState.NoInput;
        foreach (var cam in allCams) cam.SetActive(false);
        allCams[idx].SetActive(true);
        if (hidePlayer) GameState.Player.SetActive(false);
    }
}