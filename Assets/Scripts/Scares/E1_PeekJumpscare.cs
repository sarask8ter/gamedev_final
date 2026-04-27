using UnityEngine;
using System.Collections;

public class E1_PeekJumpscare : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera peekCamera;

    [Header("Scare")]
    [SerializeField] private Transform jumpscarePoint;

    [SerializeField] private float vanishDelay = 3f;
    [SerializeField] private float reappearDelay = 3f;

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
        playerCamera.enabled = false;
        peekCamera.enabled = true;

        yield return new WaitForSeconds(0.5f);

        // vanish
        neighbor.transform.position = originalPosition + Vector3.up * 100f;

        yield return new WaitForSeconds(vanishDelay);

        yield return new WaitForSeconds(reappearDelay);

        // snap to glass
        neighbor.transform.position = jumpscarePoint.position;
        neighbor.transform.rotation = jumpscarePoint.rotation;

        Debug.Log("JUMPSCARE!");

        yield return new WaitForSeconds(1.5f);

        neighbor.transform.position = originalPosition;

        peekCamera.enabled = false;
        playerCamera.enabled = true;
    }
}