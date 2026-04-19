using UnityEngine;
using System.Collections;

public class E1_PeekJumpscare : MonoBehaviour
{
    private GameObject neighbor;
    private Vector3 originalPosition;

    public void SetNeighbor(GameObject obj)
    {
        neighbor = obj;
        if (neighbor != null)
        {
            originalPosition = neighbor.transform.position;
        }
    }

    public void PlayJumpscare()
    {
        if (neighbor == null) return;

        StartCoroutine(Jumpscare());
    }

    IEnumerator Jumpscare()
    {
        // Move neighbor slightly away (away from player/camera direction)
        Vector3 jumpOffset = -neighbor.transform.right.normalized * 6f;
        neighbor.transform.position += jumpOffset;

        yield return new WaitForSeconds(1f);

        // Snap back to original spawn position
        neighbor.transform.position = originalPosition;
    }
}