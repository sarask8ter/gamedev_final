using UnityEngine;

public class MoveAndChangePhysicsMethods
{
    public static void MoveToPoint(GameObject obj, Transform movePoint, bool reparent)
    {
        if (reparent)
        {
            obj.transform.SetParent(movePoint, false);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }
        else 
        {
            obj.transform.position = movePoint.position;
            obj.transform.rotation = movePoint.rotation;
        }
    }

    static void MoveAndSetLayer(GameObject obj, string layerName, Transform movePoint, bool reparent)
    {
        MoveToPoint(obj, movePoint, reparent);
        SetLayerRecursively(obj, LayerMask.NameToLayer(layerName));
    }

    public static void MoveAndDisable(GameObject obj, string layerName, Transform movePoint, bool reparent)
    {
        MoveAndSetLayer(obj, layerName, movePoint, reparent);
        ChangePhysics(obj, false);
    }

    public static void MoveAndEnable(GameObject obj, string layerName, Transform movePoint, bool reparent)
    {
        MoveAndSetLayer(obj, layerName, movePoint, reparent);
        ChangePhysics(obj, true);
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
