using UnityEngine;

public enum PlayerState
{
    Normal,
    Inspecting,
    Dialogue,
}

public class PlayerStateManager : MonoBehaviour
{
    private PlayerState state;
    public static PlayerState State { get => _instance.state; set => _instance.state = value; }
    private static PlayerStateManager _instance;

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
}
