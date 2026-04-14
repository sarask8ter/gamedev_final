using System.Collections;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] private Transform raisedPoint;
    [SerializeField] private Transform loweredPoint;
    [SerializeField] private float raiseAndLowerDuration;
    private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine moveRoutine;
    private Renderer[] renderers;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        DisableRenderers();
    }

    public void StartCall()
    {
        StartMove(loweredPoint.position, raisedPoint.position, false);
    }

    public void EndCall()
    {
        StartMove(raisedPoint.position, loweredPoint.position, true);
    }

    void StartMove(Vector3 start, Vector3 end, bool unrenderAtEnd)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveBetweenPos(start, end, unrenderAtEnd));
    }

    private IEnumerator MoveBetweenPos(Vector3 start, Vector3 end, bool unrenderAtEnd)
    {
        EnableRenderers();

        float elapsed = 0f;

        while (elapsed < raiseAndLowerDuration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(elapsed / raiseAndLowerDuration);

            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
        moveRoutine = null;

        if (unrenderAtEnd) 
        {
            DisableRenderers();
        }
    }

    private void EnableRenderers()
    {
        foreach (var rend in renderers) rend.enabled = true;
    }

    private void DisableRenderers()
    {
        foreach (var rend in renderers) rend.enabled = false;
    }
}
