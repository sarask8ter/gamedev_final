using System.Linq;
using UnityEngine;

public class ChangeUIOnInspection : MonoBehaviour
{
    [SerializeField] private GameObject inspectInstructionsUI;

    void Update()
    {
        inspectInstructionsUI.SetActive(PlayerStateManager.State == PlayerState.Inspecting);
    }
}