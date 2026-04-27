using UnityEngine;

public class MovementHelper
{
    public static void MovePlayer(Transform playerTeleportPoint)
    {
        var player = GameState.Player;

        var controller = player.GetComponent<CharacterController>();
        var fps = player.GetComponent<StarterAssets.FirstPersonController>();

        controller.enabled = false;

        MoveToPoint(player, playerTeleportPoint, false);
        fps.SetLookRotation(
            playerTeleportPoint.eulerAngles.y,
            NormalizePitch(playerTeleportPoint.eulerAngles.x)
        );

        controller.enabled = true;
    }

    static float NormalizePitch(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    public static void MoveToDefaultLayer(GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer("Default");
    }

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
