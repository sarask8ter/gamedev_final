using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tea : MonoBehaviour, IInteractable
{
    public bool IsInteractable => EndingState.ChosenEnding == Ending.DeathByTea;
    [SerializeField] private float initialDelay;
    [SerializeField] private float fadeDuration;
    [SerializeField] private DialogueNode deathDialogue;

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
