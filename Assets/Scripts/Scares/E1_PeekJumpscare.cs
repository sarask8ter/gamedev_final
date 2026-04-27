using UnityEngine;
using System.Collections;

public class E1_PeekJumpscare : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera peekCamera;

    [Header("Scare")]
    [SerializeField] private Transform jumpscarePoint;

    [SerializeField] private float reappearDelay = 1f;
    [SerializeField] private Transform player;

    private GameObject neighbor;
    private Vector3 originalPosition;

    public void SetNeighbor(GameObject obj)
    {
        neighbor = obj;

        if (neighbor != null)
            originalPosition = neighbor.transform.position;
    }

    public void PlayJumpscare()
    {
        if (neighbor == null) return;

        StartCoroutine(JumpscareSequence());
    }

    IEnumerator JumpscareSequence()
    {
        // switch cameras
        playerCamera.gameObject.SetActive(false);
        peekCamera.gameObject.SetActive(true);

        // neighbor disappears
        neighbor.transform.position = originalPosition + Vector3.up * 100f;

        yield return new WaitForSeconds(reappearDelay);

        // snap at window
        neighbor.transform.position = jumpscarePoint.position;
        neighbor.transform.rotation = jumpscarePoint.rotation;

        Debug.Log("JUMPSCARE!");

        yield return new WaitForSeconds(1f);

        neighbor.transform.position = originalPosition;

        // move player body to peek camera position
        TeleportPlayerToPeek();

        // switch back
        peekCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }

    void TeleportPlayerToPeek()
    {
        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        Vector3 peekPos = peekCamera.transform.position;
        peekPos.x = -2f; // override only x

        player.SetPositionAndRotation(
            peekPos,
            peekCamera.transform.rotation
        );

        if (controller != null)
            controller.enabled = true;
    }
}