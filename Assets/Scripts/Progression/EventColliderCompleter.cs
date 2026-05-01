using UnityEngine;

public class EventColliderCompleter : MonoBehaviour
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