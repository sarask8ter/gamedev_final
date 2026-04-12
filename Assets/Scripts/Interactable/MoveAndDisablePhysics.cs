using UnityEngine;

public class MoveAndChangePhysicsMethods
{
    public static void MoveAndDisable(GameObject obj, string layerName, Transform movePoint)
    {
        MoveAndSetLayer(obj, layerName, movePoint);
        ChangePhysics(obj, true);
    }

    public static void MoveAndEnable(GameObject obj, string layerName, Transform movePoint)
    {
        MoveAndSetLayer(obj, layerName, movePoint);
        ChangePhysics(obj, false);
    }

    public static void MoveToPoint(GameObject obj, Transform movePoint)
    {
        obj.transform.position = movePoint.position;
        obj.transform.rotation = movePoint.rotation;
    }

    static void MoveAndSetLayer(GameObject obj, string layerName, Transform movePoint)
    {
        MoveToPoint(obj, movePoint);
        SetLayerRecursively(obj, LayerMask.NameToLayer(layerName));
    }

    static void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            child.gameObject.layer = layer;
        }
    }

    static void ChangePhysics(GameObject obj, bool enable)
    {
        var col = obj.GetComponent<Collider>();
        if (col != null) col.enabled = enable;
    }
}
