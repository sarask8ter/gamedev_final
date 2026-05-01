using UnityEngine;

public class GameState : MonoBehaviour
{

    [Header("Evidence")]
    public static bool TalkedToNeighbor;
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
