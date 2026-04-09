using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public string Description;
    public string ProgressText;
    public abstract void StartTask();

    void Start()
    {
        StartTask();
    }
}
