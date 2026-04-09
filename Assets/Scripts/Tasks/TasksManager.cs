using UnityEngine;

public class TasksManager : MonoBehaviour
{
    private static TasksManager _instance;

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
