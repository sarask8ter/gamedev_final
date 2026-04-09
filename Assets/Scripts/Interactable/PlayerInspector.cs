using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInspector : MonoBehaviour
{

    [SerializeField] private Transform inspectPoint;
    public Transform InspectPoint => inspectPoint;
    [SerializeField] private Camera inspectCam;
    [SerializeField] private float inspectRotateSpeed;

    private GameObject inspectedItem;
    private string inspectItemLayer = "InspectedItem";
    private InputAction cancelAction;
    private InputAction lookAction;
    private InputAction clickAction;

    private static PlayerInspector _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            cancelAction = InputSystem.actions.FindAction("Cancel");
            lookAction = InputSystem.actions.FindAction("Look");
            clickAction = InputSystem.actions.FindAction("Click");
        }
    }

    void Update()
    {
        if (PlayerStateManager.State == PlayerState.Inspecting)
        {
            if (cancelAction.WasPressedThisFrame()) EndInspection();
            else if (clickAction.IsPressed()) RotateInspectedObj();
        }
    }

    public static void BeginInspection(GameObject objToInspect)
    {
        PlayerStateManager.State = PlayerState.Inspecting;
        _instance.inspectedItem = Instantiate(objToInspect, _instance.inspectPoint.position, _instance.inspectPoint.rotation);
        _instance.SetLayerRecursively(_instance.inspectedItem, LayerMask.NameToLayer(_instance.inspectItemLayer));
        _instance.DisablePhysics(_instance.inspectedItem);
    }

    public static void EndInspection()
    {
        Destroy(_instance.inspectedItem);
        _instance.inspectedItem = null;
        PlayerStateManager.State = PlayerState.Normal;
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            child.gameObject.layer = layer;
        }
    }

    void DisablePhysics(GameObject obj)
    {
        foreach (var col in obj.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    void RotateInspectedObj()
    {
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();

        float deltaRotationX = -lookDelta.x;
        float deltaRotationY = lookDelta.y;

        inspectedItem.transform.rotation = 
            Quaternion.AngleAxis(deltaRotationX * inspectRotateSpeed * Time.deltaTime, inspectCam.transform.up) *
            Quaternion.AngleAxis(deltaRotationY * inspectRotateSpeed * Time.deltaTime, inspectCam.transform.right) *
            inspectedItem.transform.rotation;
    }
}
