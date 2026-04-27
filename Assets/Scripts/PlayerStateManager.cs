using UnityEngine;

public enum PlayerState
{
    Fading, // Fading screen in or out -- disable inputs.
    Normal,
    Inspecting,
    Dialogue,
}

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerState State;
}
