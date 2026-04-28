using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TimedTask", menuName = "Event Actions/Tasks/Timed")]
public class TimedTask : Task
{
    [SerializeField] private int totalTime;
    private int currTime;

    protected override void PreStartTask()
    {
        currTime = totalTime;
        UpdateProgressText();
    }

    protected override void PostStartTask()
    {
        CoroutineHelper.StartCoroutineHelper(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (currTime > 0)
        {
            HandleProgress();
            yield return new WaitForSeconds(1f);
            if (PlayerStateManager.State != PlayerState.Inspecting) currTime--;
        }

        HandleProgress();
        CompleteTask();
    }

    protected void HandleProgress()
    {
        UpdateProgressText();
        TasksEvents.OnTaskProgress?.Invoke(CompileTaskData());
    }

    void UpdateProgressText()
    {
       progressText = "(" + currTime + ")";  
    }
}
