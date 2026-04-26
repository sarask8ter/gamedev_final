using UnityEngine;

public class DialogueContext : MonoBehaviour
{
    private static DialogueContext _instance;
    [SerializeField] private GameObject player;
    public static GameObject Player => _instance.player;

    [SerializeField] private Transform playerPeekTeleportPoint;
    public static Transform PlayerPeekTeleportPoint => _instance.playerPeekTeleportPoint;

    [SerializeField] private GameObject neighbor;
    public static GameObject Neighbor => _instance.neighbor;

    [SerializeField] private Transform neighborPeekTeleportPoint;
    public static Transform NeighborPeekTeleportPoint => _instance.neighborPeekTeleportPoint;

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
