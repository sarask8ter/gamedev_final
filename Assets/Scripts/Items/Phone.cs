using System.Collections;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] private Transform raisedPoint;
    [SerializeField] private Transform loweredPoint;
    [SerializeField] private float raiseAndLowerDuration;
    private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine moveRoutine;

    public void StartCall()
    {
        StartMove(loweredPoint.position, raisedPoint.position);
    }

    public void EndCall()
    {
        StartMove(raisedPoint.position, loweredPoint.position);
    }

    void StartMove(Vector3 start, Vector3 end)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveBetweenPos(start, end));
    }

    private IEnumerator MoveBetweenPos(Vector3 start, Vector3 end)
    {
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
    }
}
