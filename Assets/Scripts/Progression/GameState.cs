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

    [Header("Neighbor")]
    [SerializeField] private GameObject neighbor;
    public static GameObject Neighbor => _instance.neighbor;
    [SerializeField] private Transform neighborPeekTeleportPoint;
    public static Transform NeighborPeekTeleportPoint => _instance.neighborPeekTeleportPoint;
    [SerializeField] private Transform neighborAtNeighborHouseInitialTeleportPoint;
    public static Transform NeighborAtNeighborHouseInitialTeleportPoint => _instance.neighborAtNeighborHouseInitialTeleportPoint;

    // DYNAMIC
    [Header("Evidence")]
    public static bool FoundEvidence;

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
