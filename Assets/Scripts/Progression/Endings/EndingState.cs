using UnityEngine;

public enum Ending
{
    None,
    DeathByTea,
    LeaveWithNoEvidence,
    LeaveAndCallPolice,
    SolveCase,
}

public class EndingState : MonoBehaviour
{
    private static EndingState _instance;
    
    private Ending chosenEnding = Ending.None;
    public static Ending ChosenEnding { get => _instance.chosenEnding; set => _instance.chosenEnding = value; }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}