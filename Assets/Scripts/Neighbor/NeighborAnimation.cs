using UnityEngine;

public class NeighborAnimation : MonoBehaviour
{
    private Animator animator;
    private static readonly int DeathTrigger = Animator.StringToHash("death");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        ProgressManager.OnProgressEventStarted += OnProgressEventStarted;
    }

    void OnDestroy()
    {
        ProgressManager.OnProgressEventStarted -= OnProgressEventStarted;
    }

    void OnProgressEventStarted(ProgressEvent evt)
    {
        if (evt == ProgressEvent.NeighborDeath)
        {
            PlayDeath();
        }
    }

    public void PlayDeath()
    {
        animator.SetTrigger(DeathTrigger);
    }
}
