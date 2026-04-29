using UnityEngine;

public class NeighborAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem powerTransferVFX;

    private Animator animator;
    private static readonly int DeathTrigger = Animator.StringToHash("death");

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        // Disable auto-play on the particle system
        if (powerTransferVFX != null)
        {
            var main = powerTransferVFX.main;
            main.playOnAwake = false;
            powerTransferVFX.Stop();
        }
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
        Debug.Log($"NeighborAnimation: Event started: {evt}");
        
        if (evt == ProgressEvent.NeighborConfrontation)
        {
            if (powerTransferVFX != null)
            {
                
                powerTransferVFX.Play();
                Debug.Log($"NeighborAnimation: VFX played");
            }
            else
            {
                Debug.LogError("powerTransferVFX is null!");
            }
        }
        else if (evt == ProgressEvent.NeighborDeath)
        {
            PlayDeath();
        }
    }

    public void PlayDeath()
    {
        animator.SetTrigger(DeathTrigger);
    }
}


