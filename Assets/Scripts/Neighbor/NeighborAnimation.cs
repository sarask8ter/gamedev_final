using System.Collections;
using UnityEngine;

public class NeighborAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem powerTransferVFX;
    [SerializeField] private float deathEndingDelay;
    [SerializeField] private GameObject powerTransferSFX;
    [SerializeField] private GameObject deathSFX;


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
        if (evt == ProgressEvent.NeighborAttack)
        {
            if (powerTransferVFX != null)
            {
                
                powerTransferVFX.Play();
                if (powerTransferSFX != null) Instantiate(powerTransferSFX);
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
            StartCoroutine(EndSceneAfterDelay());
        }
    }

    public void PlayDeath()
    {
        animator.SetTrigger(DeathTrigger);
        if (deathSFX != null) Instantiate(deathSFX);
    }

    private IEnumerator EndSceneAfterDelay()
    {
        yield return new WaitForSeconds(deathEndingDelay);
        TriggerEnd.End();
    }
}


