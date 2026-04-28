using UnityEngine;

public class GameState : MonoBehaviour
{
    // IMMUTABLE/STATIC
    [Header("Player")]
    [SerializeField] private GameObject player;
    public static GameObject Player => _instance.player;
    [SerializeField] private Transform playerPeekTeleportPoint;
    public static Transform PlayerPeekTeleportPoint => _instance.playerPeekTeleportPoint;
    [SerializeField] private Transform playerAtNeighborHouseInitialTeleportPoint;
    public static Transform PlayerAtNeighborHouseInitialTeleportPoint => _instance.playerAtNeighborHouseInitialTeleportPoint;
    [SerializeField] private Transform playerTeaTeleportPoint;
    public static Transform PlayerTeaTeleportPoint => _instance.playerTeaTeleportPoint;

    [Header("Neighbor")]
    [SerializeField] private GameObject neighbor;
    public static GameObject Neighbor => _instance.neighbor;
    [SerializeField] private Transform neighborPeekTeleportPoint;
    public static Transform NeighborPeekTeleportPoint => _instance.neighborPeekTeleportPoint;
    [SerializeField] private Transform neighborAtNeighborHouseInitialTeleportPoint;
    public static Transform NeighborAtNeighborHouseInitialTeleportPoint => _instance.neighborAtNeighborHouseInitialTeleportPoint;
    [SerializeField] private Transform neighborTeaTeleportPoint;
    public static Transform NeighborTeaTeleportPoint => _instance.neighborTeaTeleportPoint;
    [SerializeField] private Transform neighborBathroomTeleportPoint;
    public static Transform NeighborBathroomTeleportPoint => _instance.neighborBathroomTeleportPoint;

    [Header("Others")]
    [SerializeField] private GameObject[] objectsToActivateAtTeaReady;
    public static GameObject[] ObjectsToActivateAtTeaReady => _instance.objectsToActivateAtTeaReady;
    [SerializeField] private DoorPivot neighborDoor;
    public static DoorPivot NeighborDoor => _instance.neighborDoor;
    [SerializeField] private GameObject stairBlocker;
    public static GameObject StairBlocker => _instance.stairBlocker;
    [SerializeField] private GameObject corpse;
    public static GameObject Corpse => _instance.corpse;
    [SerializeField] private Transform corpseUprightPoint;
    public static Transform CorpseUprightPoint => _instance.corpseUprightPoint;

    // DYNAMIC
    [Header("Evidence")]
    public static bool FoundEvidence = true;

    private static GameState _instance;
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }    
}
