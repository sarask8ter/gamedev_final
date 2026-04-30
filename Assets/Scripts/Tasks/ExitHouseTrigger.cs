using UnityEngine;

public class ExitHouseTrigger : MonoBehaviour
{
    [SerializeField] private DoorPivot frontDoor;
    [SerializeField] private E1_NeighborEvent neighborEvent;

    private bool hasTriggeredExit;

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggeredExit) return;
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player entered exit trigger");

        // if (!frontDoor.IsOpen)
        // {
        //     Debug.Log("Front door isOpen: " + frontDoor.IsOpen);
        //     Debug.Log("Door is closed — cannot exit");
        //     return;
        // }

        hasTriggeredExit = true;

        neighborEvent.TriggerOutsideSequence();
    }
}