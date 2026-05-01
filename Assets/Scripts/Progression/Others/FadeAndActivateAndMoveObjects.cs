using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectToTransformPair
{
    public GameObject obj;
    public Transform transform;
}


public class FadeAndActivateAndMoveObjects : EventAction
{
    [SerializeField] private GameObject[] objsToActivate;
    [SerializeField] private GameObjectToTransformPair[] objsToMove;
    [SerializeField] private float fadeDuration;
    private Dictionary<GameObject, Transform> objsToMoveDict;

    void Awake()
    {
        objsToMoveDict = new Dictionary<GameObject, Transform>();
        foreach (var pair in objsToMove)
        {
            objsToMoveDict[pair.obj] = pair.transform;
        }
    }

    public override void OnEventStart()
    {
        CoroutineHelper.StartCoroutineHelper(FadeAndTeleport());
    }

    IEnumerator FadeAndTeleport()
    {
        PlayerStateManager.State = PlayerState.NoInput;

        yield return ScreenFader.FadeOut(fadeDuration);

        TeleportObjects();
        foreach (var obj in objsToActivate) obj.SetActive(true);

        yield return ScreenFader.FadeIn(fadeDuration);

        PlayerStateManager.State = PlayerState.Normal;
        CompleteEvent();
    }

    void TeleportObjects()
    {
        foreach (var (obj, point) in objsToMoveDict)
        {
            if (obj == GameState.Player) MovementHelper.MovePlayer(point);
            else MovementHelper.MoveToPoint(obj, point, false);
        }
    }
}