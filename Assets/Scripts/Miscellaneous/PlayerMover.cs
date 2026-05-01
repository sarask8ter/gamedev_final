using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(StarterAssets.FirstPersonController))]
public class PlayerMover : MonoBehaviour
{
    private CharacterController characterController;
    private StarterAssets.FirstPersonController fps;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        fps = GetComponent<StarterAssets.FirstPersonController>();
    }

    public void MovePlayer(Transform playerTeleportPoint)
    {
        characterController.enabled = false;

        MovementHelper.MoveToPoint(gameObject, playerTeleportPoint, false);
        fps.SetLookRotation(
            playerTeleportPoint.eulerAngles.y,
            NormalizePitch(playerTeleportPoint.eulerAngles.x)
        );

        characterController.enabled = true;
    }

    static float NormalizePitch(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}