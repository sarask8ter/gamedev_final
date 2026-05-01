using UnityEngine;
using System.Collections;

public class FlickerLights : MonoBehaviour
{
    [SerializeField] private MainDoor frontDoor;
    [SerializeField] private SpiritController spiritController;
    [SerializeField] private DialogueNode lightsFlickerReaction;
    [SerializeField] private float flickerDelay;

    void Start()
    {
        ProgressManager.SubscribeToStart(ProgressEvent.LightsFlicker, () =>
        {
            if (frontDoor != null) frontDoor.SetOpen(false);
            StartCoroutine(HauntSequence());
        });
    }

    IEnumerator HauntSequence()
    {
        if (GameState.TalkedToNeighbor)
        {
            yield return new WaitForSeconds(flickerDelay);
            spiritController.TriggerEvent(SpiritEventType.FlickerLights);
            Debug.Log("Flicker lights");
            yield return new WaitForSeconds(0.5f);
            DialogueManager.StartDialogue(lightsFlickerReaction, true, true, () =>
            {
                ProgressManager.CompleteEvent(ProgressEvent.LightsFlicker);
            });
        } else
        {
            ProgressManager.CompleteEvent(ProgressEvent.LightsFlicker);
        }
    }
}