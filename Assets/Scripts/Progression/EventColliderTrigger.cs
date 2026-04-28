using UnityEngine;

public class EventColliderTrigger : MonoBehaviour
{
    [SerializeField] private ProgressEvent progressEvent;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ProgressManager.CompleteEvent(progressEvent);
            gameObject.SetActive(false);
        }
    }
}