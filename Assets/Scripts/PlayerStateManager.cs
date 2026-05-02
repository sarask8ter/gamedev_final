using UnityEngine;

public enum PlayerState
{
    NoInput,
    OnlyLookingInput,
    Normal,
    Inspecting,
    Dialogue,
    Pause,
}

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerState State;
}
