using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tea : MonoBehaviour, IInteractable
{
    public bool IsInteractable => EndingState.ChosenEnding == Ending.DeathByTea;
    [SerializeField] private Task drinkTeaTask;
    [SerializeField] private float initialDelay;
    [SerializeField] private float fadeDuration;
    [SerializeField] private DialogueNode deathDialogue;

    void Start()
    {
        ProgressManager.SubscribeToStart(ProgressEvent.StayTheNightDecision, () =>
        {
           if (IsInteractable)
            {
                // So death by tea ending was chosen.
                drinkTeaTask.OnEventStart();
            }
        });
    }

    public void Interact(PlayerInteractor player)
    {
        MovementHelper.MoveAndDisable(gameObject, player.PickedUpLayerName, player.HoldingPoint, true);
        StartCoroutine(DieSequence());
    }

    IEnumerator DieSequence()
    {
        yield return new WaitForSeconds(initialDelay);
        ScreenFader.FadeOut(fadeDuration);
        DialogueManager.StartDialogue(deathDialogue);
    }
}
