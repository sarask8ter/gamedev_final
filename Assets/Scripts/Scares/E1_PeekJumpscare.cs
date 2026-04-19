using UnityEngine;
using System.Collections;

public class E1_PeekJumpscare : MonoBehaviour
{
    private GameObject neighbor;

    public void SetNeighbor(GameObject obj)
    {
        neighbor = obj;
    }

    public void PlayJumpscare()
    {
        if (neighbor == null) return;

        StartCoroutine(Jumpscare());
    }

    IEnumerator Jumpscare()
    {
        var renderers = neighbor.GetComponentsInChildren<Renderer>();

        foreach (var r in renderers)
            r.enabled = false;

        yield return new WaitForSeconds(2f);

        foreach (var r in renderers)
            r.enabled = true;

        neighbor.transform.position += neighbor.transform.forward.normalized * 1.5f;
    }
}